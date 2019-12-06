using Domain.Users.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Repository.Mongo.Infrastructure
{
    public class MongoDbParams : IDbParams
    {
        public string ConnectionString { get; private set; }

        public MongoDbParams(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("Mongo");
        }
    }
}
