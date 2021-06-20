using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatInc.App.Jwt
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/api/users/login";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(15);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
