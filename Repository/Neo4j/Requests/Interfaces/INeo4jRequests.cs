
namespace Repository.Neo4j.Requests.Interfaces
{
    public interface INeo4jRequests
    {
        string AutoIncrementor_MergeRequest { get; }

        string AutoIncrementor_GetNewPostId_SetRequest { get; }

        string AutoIncrementor_GetNewUserId_SetRequest { get; }


        string Post_CreatePost_CreateRequest { get; }

        string Post_DeletePost_MatchRequest { get; }

        string Post_GetPost_MatchRequest { get; }

        string Create_UserEmail_UniqueConstraint { get; }
    }
}
