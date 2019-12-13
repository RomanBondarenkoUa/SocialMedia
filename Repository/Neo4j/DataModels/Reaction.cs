using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Neo4j.DataModels
{
    public class Reaction
    {
        public string Text { get; set; }

        public bool? IsPositive { get; set; }

    }
}
