
using Repository.Dapper.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Mongo.Helpers
{
    public static class ReactionHelper
    {
        public static int AggregatePositiveReactionsRate(this IEnumerable<Mongo.DataModels.Reaction> comments)
        {
            return comments.Select(comm =>
            {
                if (comm.IsPositive.HasValue)
                {
                    return comm.IsPositive.Value ? 1 : 0;
                }
                return 0;
            }).Sum();
        }

        public static int AggregateNegativeReactionsRate(this IEnumerable<Mongo.DataModels.Reaction> comments)
        {
            return comments.Select(comm =>
            {
                if (comm.IsPositive.HasValue)
                {
                    return !comm.IsPositive.Value ? 1 : 0;
                }
                return 0;
            }).Sum();
        }
    }
}
