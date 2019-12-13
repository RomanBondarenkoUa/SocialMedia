
namespace API.Infrastructure.Models
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
