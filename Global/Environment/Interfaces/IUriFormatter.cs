using System;

namespace Global.Environment.Interfaces
{
    public interface IUriFormatter
    {
        Uri GetUserUri(int userId);

        Uri GetPostUri(long postId);
    }
}
