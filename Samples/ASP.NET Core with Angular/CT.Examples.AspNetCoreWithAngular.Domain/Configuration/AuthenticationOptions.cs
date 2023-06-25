using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CT.Examples.AspNetCoreWithAngular.Domain.Configuration;

public class AuthenticationOptions
{
    public AuthenticationOptions(string jwtKey)
    {
        Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    }

    public SymmetricSecurityKey Key { get; set; }
}
