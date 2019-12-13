using Microsoft.Extensions.Configuration;
using Repository.Neo4j.Infrastructure.Model;
using System;
using System.Linq;

namespace Repository.Neo4j.Infrastructure
{
    public class Neo4jDbParams : INeo4jDbParams
    {
        private Neo4jConnectionModel connectionModel;

        public Neo4jDbParams(IConfiguration configuration)
        {
            var neo4jSection = configuration.GetSection("ConnectionStrings").GetSection("Neo4j");

            this.connectionModel = new Neo4jConnectionModel
            {
                ConnectionUri = new Uri(neo4jSection["connectionUri"], UriKind.Absolute),
                UserName = neo4jSection["userName"],
                UserPassword = neo4jSection["userPassword"],
            };
        }

        public Neo4jConnectionModel GetNeo4jConnection()
        {
            return this.connectionModel;
        }
    }
}
