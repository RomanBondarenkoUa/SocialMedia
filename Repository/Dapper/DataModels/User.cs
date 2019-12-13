
using System;

namespace Repository.Dapper.DataModels
{
    public class User
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string AboutMe { get; set; }

        public DateTime RegistredAt { get; set; }
    }
}
