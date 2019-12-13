using Domain.Users.SocialActivities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Helpers
{
    public static class CommentHelper
    {
        public static int AggregatePositiveCommentsRate(this IEnumerable<Comment> comments)
        {
            return comments.Select(comm =>
            {
                if (comm.IsPositiveComment.HasValue)
                {
                    return comm.IsPositiveComment.Value ? 1 : 0;
                }
                return 0;
            }).Sum();
        }

        public static int AggregateNegativeCommentsRate(this IEnumerable<Comment> comments)
        {
            return comments.Select(comm =>
            {
                if (comm.IsPositiveComment.HasValue)
                {
                    return !comm.IsPositiveComment.Value ? 1 : 0;
                }
                return 0;
            }).Sum();
        }
    }
}
