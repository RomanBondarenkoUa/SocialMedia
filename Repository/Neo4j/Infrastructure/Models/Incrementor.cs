using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Neo4j.Infrastructure.Models
{
    public class Incrementor
    {
        public int? LatestUserId { get; set; }

        public long? LatestPostId { get; set; }
    }
}
