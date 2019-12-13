using Repository.Neo4j.Requests.Interfaces;
using System;

namespace Repository.Neo4j.Requests
{
    public class Neo4jRequests : INeo4jRequests
    {
        public string AutoIncrementor_MergeRequest => "(increment:AutoIncrementor) ";

        public string AutoIncrementor_GetNewPostId_SetRequest =>
            "increment.LatestPostId = CASE " +
                "WHEN NOT EXISTS(increment.LatestPostId) " +
                    "THEN 0 " +
                "WHEN EXISTS(increment.LatestPostId) " +
                    "THEN increment.LatestPostId + 1 " +
            "END";

        public string AutoIncrementor_GetNewUserId_SetRequest => 
            "increment.LatestUserId = CASE " +
                "WHEN NOT EXISTS(increment.LatestUserId) " +
                    "THEN 0 " +
                "WHEN EXISTS(increment.LatestUserId) " +
                    "THEN increment.LatestUserId + 1 " +
            "END ";

        public string Post_CreatePost_CreateRequest => throw new NotImplementedException();

        public string Post_DeletePost_MatchRequest => throw new NotImplementedException();

        public string Post_GetPost_MatchRequest => throw new NotImplementedException();

        public string Create_UserEmail_UniqueConstraint => "";
    }
}
