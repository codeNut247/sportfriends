using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CodeNut247.DotNetCore.ApiAuth
{
    public class JwtBuilder
    {
        private Claim[] _claims;
        private SymmetricSecurityKey _key;
        private string _algorithm;
        private SigningCredentials _credentials;
        private DateTime _expires = DateTime.Now.AddDays(1);


        public JwtBuilder(Claim[] claims, string key, DateTime expires, string algorithm = SecurityAlgorithms.HmacSha512Signature)
        {
            _claims = claims ?? throw new ArgumentNullException(nameof(claims));
            _key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(key ?? throw new ArgumentNullException(nameof(key))));
            _algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
            _credentials = new SigningCredentials(this._key, this._algorithm);
            _expires = expires;
        }

        public string BuildToken()
        {
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(this._claims),
                Expires = this._expires,
                SigningCredentials = this._credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}