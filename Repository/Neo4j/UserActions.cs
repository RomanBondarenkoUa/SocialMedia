using Domain.Users.Interfaces;
using Global.Environment.Interfaces;
using Neo4jClient;
using Neo4jClient.Cypher;
using Repository.Dapper.Mappers.Interfaces;
using Repository.Neo4j.Infrastructure;
using Repository.Neo4j.Infrastructure.Models;
using Repository.Neo4j.Requests.Interfaces;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Neo4j
{
    public class UserActions : IUserActions
    {
        private GraphClient client;
        private Neo4j.Mappers.Interfaces.IUserMapper userMapper;
        private INeo4jRequests neo4jRequests;
        private IUriFormatter uriFormatter;
        private IDateTimeProvider dateTimeProvider;

        public UserActions(
            INeo4jDbParams neo4JDbParams,
            Neo4j.Mappers.Interfaces.IUserMapper userMapper,
            INeo4jRequests neo4jRequests,
            IUriFormatter uriFormatter,
            IDateTimeProvider dateTimeProvider)
        {
            var neo4jCreds = neo4JDbParams.GetNeo4jConnection();

            this.client = new GraphClient(neo4jCreds.ConnectionUri, neo4jCreds.UserName, neo4jCreds.UserPassword);
            this.userMapper = userMapper;
            this.neo4jRequests = neo4jRequests;
            this.uriFormatter = uriFormatter;
            this.dateTimeProvider = dateTimeProvider;
        }

        public bool CheckUser(string email, string passwordHash)
        {
            this.client.Connect();

            var resultUser = this.client.Cypher
                .Match("(user:User)")
                .Where((Neo4j.DataModels.User user) => user.Email == email && user.PasswordHash == passwordHash)
                .Return(user => user.As<Neo4j.DataModels.User>())
                .Results
                .FirstOrDefault();

            return resultUser == null ? false : true;
        }

        public void CreateUser(string name, string email, string aboutMe, string passwordHash)
        {
            this.client.Connect();

            var user = new Neo4j.DataModels.User()
            {
                Id = this.GetNewUserId(),
                Name = name,
                Email = email,
                AboutMe = aboutMe,
                PasswordHash = passwordHash,
                RegistredAt = this.dateTimeProvider.Now(),
            };

            this.client.Cypher
                .Create("(user:User {newUser})")
                .WithParam("newUser", user)
                .ExecuteWithoutResults();

            this.client.Cypher
                .CreateUniqueConstraint("user:User", "user.Email")
                .ExecuteWithoutResults();
        }

        public void DeleteUser(string email)
        {


            this.client.Cypher
                .Match("(user:User)")
                .Where((Neo4j.DataModels.User user) => user.Email == email)
                .DetachDelete("user")
                .ExecuteWithoutResults();
        }

        public User GetUser(string email, bool includeUserSubscriptions)
        {
            User resultUser = null;
            if (includeUserSubscriptions)
            {
                resultUser = this.GetUserWithSubscriptions(email);
            }
            else
            {
                resultUser = this.GetUserWithoutSubscriptions(email);
            }

            return resultUser;
        }

        public IEnumerable<long> GetUserPostIds(string userEmail)
        {
            this.client.Connect();

            return this.client.Cypher
                .Match("(user:User)-[:CREATE_POST]->(post:Post)")
                .Where((Neo4j.DataModels.User user) => user.Email == userEmail)
                .Return(post => post.As<Neo4j.DataModels.Post>().Id)
                .Results;
        }

        public void Subscribe(string subscriberEmail, int publisherId)
        {
            this.client.Connect();

            this.client.Cypher
                .Match("(publisher:User)", "(subscriber:User)")
                .Where((Neo4j.DataModels.User publisher) => publisher.Id == publisherId)
                .AndWhere((Neo4j.DataModels.User subscriber) => subscriber.Email == subscriberEmail)
                .CreateUnique("(subscriber)-[:SUBSCRIBED]->(publisher)")
                .ExecuteWithoutResults();
        }

        public void UpdateUser(string email, string newEmail, string newPasswordHash)
        {
            throw new NotImplementedException();
        }

        private int GetNewUserId()
        {
            client.Connect();
            return client.Cypher
                .Merge(this.neo4jRequests.AutoIncrementor_MergeRequest)
                .Set(this.neo4jRequests.AutoIncrementor_GetNewUserId_SetRequest)
                .Return(increment => increment.As<Incrementor>().LatestUserId)
                .Results
                .First()
                .Value;
        }

        private User GetUserWithSubscriptions(string email)
        {
            this.client.Connect();

            var userAndSubscriptions = this.client.Cypher
                 .Match("(user:User)-[:SUBSCRIBED]->(publisher:User)")
                 .Where((Neo4j.DataModels.User user) => user.Email == email)
                 .Return((user, publisher) =>
                     new
                     {
                         user = user.As<Neo4j.DataModels.User>(),
                         subscriptions = publisher.CollectAs<Neo4j.DataModels.User>()
                     })
                 .Results
                 .FirstOrDefault();

            if (userAndSubscriptions?.user == null)
            {
                return this.GetUserWithoutSubscriptions(email);
            }

            return new User
            {
                Name = userAndSubscriptions.user?.Name,
                AboutMe = userAndSubscriptions.user?.AboutMe,
                RegistredAt = userAndSubscriptions.user.RegistredAt,
                Subscriptions = this.userMapper.Map(userAndSubscriptions?.subscriptions),
            };
        }

        private User GetUserWithoutSubscriptions(string email)
        {
            this.client.Connect();

            var userWithoutSubscriptions = this.client.Cypher
                 .Match("(user:User)")
                 .Where((Neo4j.DataModels.User user) => user.Email == email)
                 .Return((user, publisher) => user.As<Neo4j.DataModels.User>())
                 .Results
                 .FirstOrDefault();

            if (userWithoutSubscriptions == null)
            {
                throw new ArgumentNullException($"User with email {email} does not exist.");
            }

            return new User
            {
                Name = userWithoutSubscriptions?.Name,
                AboutMe = userWithoutSubscriptions?.AboutMe,
                RegistredAt = userWithoutSubscriptions.RegistredAt,
            };
        }
    }
}
