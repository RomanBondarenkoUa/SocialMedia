using System;
using System.Collections.Generic;

namespace SocialMedia.Domain.Users
{
    public class User
    {
        public string Name { get; set; }

        public string AboutMe { get; set; }

        public DateTime RegistredAt { get; set; }

        public IEnumerable<User> Subscriptions { get; set; }
    }
}