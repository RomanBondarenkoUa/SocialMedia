using Global.Environment.Interfaces;
using System;

namespace Global.Environment
{
    public class UriFormatter : IUriFormatter
    {
        public const string PostEndpointsRelativeUri = "/post/{0}";
        public const string UserEndpointsRelativeUri = "/user/{0}";

        public Uri GetPostUri(long postId)
        {
            return new Uri(string.Format(PostEndpointsRelativeUri, postId), UriKind.Relative);
        }

        public Uri GetUserUri(int userId)
        {
            return new Uri(string.Format(UserEndpointsRelativeUri, userId), UriKind.Relative);
        }
    }
}
