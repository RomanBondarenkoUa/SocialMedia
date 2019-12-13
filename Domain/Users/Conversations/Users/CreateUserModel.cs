using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users.Conversations.Users
{
    public class CreateUserModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string AboutMe { get; set; }

        public string PasswordHash { get; set; }
    }
}
