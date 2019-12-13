using Domain.Users.Interfaces;
using Global.Environment;
using Global.Environment.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Dapper;
using Repository.Dapper.Infrastructure;
using Repository.Dapper.Mappers;
using Repository.Dapper.Mappers.Interfaces;
using Repository.Mongo.Infrastructure;
using Repository.Mongo.Infrastructure.Interfaces;
using Repository.Neo4j.Infrastructure;
using Repository.Neo4j.Requests;
using Repository.Neo4j.Requests.Interfaces;
using Services;
using Services.Interfaces;
using SocialMedia.Domain.Users.Interfaces;

namespace IoC.CompositionRoot
{
    public static class CompositionRoot
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
        {

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<Services.Mappers.Interfaces.IPostMapper, Services.Mappers.PostMapper>();
            services.AddTransient<IUriFormatter, UriFormatter>();

            ConfigureNeo4j(services);
        }

        public static void ConfigureDapper(this IServiceCollection services)
        {
            services.AddTransient<IDbParams, DbParams>();
            services.AddTransient<IUserActions, UserActions>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IPostMapper, PostMapper>();
            services.AddTransient<IPostActions, PostActions>();
        }

        public static void ConfigureMongo(this IServiceCollection services)
        {
            services.AddTransient<IDbParams, Repository.Mongo.Infrastructure.MongoDbParams>();
            services.AddTransient<IUserActions, Repository.Mongo.UserActions>();
            services.AddTransient<Repository.Mongo.Mappers.Interfaces.IUserMapper, Repository.Mongo.Mappers.UserMapper>();
            services.AddTransient<IPostActions, Repository.Mongo.PostActions>();
            services.AddTransient<Repository.Mongo.Mappers.Interfaces.IPostMapper, Repository.Mongo.Mappers.PostMapper>();
            services.AddTransient<IPostIdCounter, PostIdCounter>();
            services.AddTransient<IUserIdCounter, UserIdCounter>();
        }

        public static void ConfigureNeo4j(this IServiceCollection services)
        {
            services.AddTransient<INeo4jRequests, Neo4jRequests>();
            services.AddTransient<INeo4jDbParams, Neo4jDbParams>();
            services.AddTransient<IUserActions, Repository.Neo4j.UserActions>();
            services.AddTransient<IPostActions, Repository.Neo4j.PostActions>();
            services.AddTransient<Repository.Neo4j.Mappers.Interfaces.IUserMapper, Repository.Neo4j.Mappers.UserMapper>();
            services.AddTransient<Repository.Neo4j.Mappers.Interfaces.IPostMapper, Repository.Neo4j.Mappers.PostMapper>();
        }
    }
}
