using Facebook.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Utilities
{
    public class Jwt : IJwt
    {
		string JwtSecretKey;
		public Jwt(IConfiguration configuration)
		{
			JwtSecretKey = configuration["Jwt:SecretKey"];
		}
        public string GenerateToken(int userId)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


		public bool ValidateCurrentToken(string token)
		{
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecretKey));

			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = false,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					IssuerSigningKey = mySecurityKey,
					
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public string GetId(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			var stringClaimValue = securityToken.Claims.First(claim => claim.Type == "nameid").Value;
			return stringClaimValue;
		}

	}
}
