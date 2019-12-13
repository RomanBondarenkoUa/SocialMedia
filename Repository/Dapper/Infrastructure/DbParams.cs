using Domain.Users.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Repository.Dapper.Infrastructure
{
    public class DbParams : IDbParams
    {
        public string ConnectionString { get; private set; }

        public DbParams(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("Dapper");
        }
    }
}
