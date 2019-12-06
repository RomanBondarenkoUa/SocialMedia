using Microsoft.IdentityModel.Tokens;

namespace API.Auth.Jwt.Interfaces
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
