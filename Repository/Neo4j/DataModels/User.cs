using System;

namespace Repository.Neo4j.DataModels
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string AboutMe { get; set; }

        public DateTime RegistredAt { get; set; }

        public string PasswordHash { get; set; }
    }
}
