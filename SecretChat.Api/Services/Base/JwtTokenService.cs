using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SecretChat.Api.Services.Base
{
	public class JwtTokenService(IConfiguration configuration)
	{
		public string GenerateJwtToken(User user)
		{
			var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!);
			var symmetricKey = new SymmetricSecurityKey(key);
			var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
			var issuer = configuration.GetValue<string>("Jwt:Issuer");
			var audience = configuration.GetValue<string>("Jwt:Audience");
			Claim[] claims = [
				 new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				 new(ClaimTypes.Name, user.Name),
				 new(ClaimTypes.Email, user.Email)
				];
			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
