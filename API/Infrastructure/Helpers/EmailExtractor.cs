
using System;
using System.Linq;
using System.Security.Claims;

namespace API.Infrastructure.Helpers
{
    public static class EmailExtractor
    {
        public static string ExtractEmail(this ClaimsPrincipal claims)
        {
            var email = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if(email == null)
            {
                throw new InvalidOperationException($"{ClaimTypes.Email} is required.");
            }

            return email;
        }
    }
}
