using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Neo4j.Infrastructure.Models
{
    internal class ComposedPost
    {
        public Neo4j.DataModels.User Publisher;

        public Neo4j.DataModels.Post Post;

        public IEnumerable<Neo4j.DataModels.Reaction> Reactions;

        public IEnumerable<Neo4j.DataModels.User> Commentators;
    }
}
