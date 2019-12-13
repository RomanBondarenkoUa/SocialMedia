using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Neo4j.Infrastructure.Model
{
    public class Neo4jConnectionModel
    {
        public Uri ConnectionUri { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }
    }
}
