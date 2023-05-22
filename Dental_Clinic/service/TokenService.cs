using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dental_Clinic.service
{
    public class TokenService : ITokenService
    {
        private ITokenRepository tRepository;
        private IUserRepository uRepository;

        public TokenService(ITokenRepository tRepository, IUserRepository uRepository)
        {
            this.tRepository = tRepository;
            this.uRepository = uRepository;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey123"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey123")),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public TokenModel RefreshToken(TokenModel model)
        {
            if (model.AccessToken == null || model.RefreshToken == null) throw new ArgumentNullException("Ошибка запроса");

            string accessToken = model.AccessToken;
            string refreshToken = model.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            string login = principal.Identity.Name;

            User user = uRepository.findAll().SingleOrDefault(u => u.login == login);

            if (user == null || user.refreshToken.token != refreshToken || user.refreshToken.lifeTime <= DateTime.Now)
                throw new ArgumentException("Ошибка запроса");

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            user.refreshToken.token = newRefreshToken;
            update(user.refreshToken);

            return new TokenModel()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public void RevokeToken(string login)
        {
            User user = uRepository.findAll().SingleOrDefault(u => u.login == login);
            if (user == null) throw new ArgumentException("Ошибка запроса");
            user.refreshToken = null;
            uRepository.update(user);
        }

        public void update(RefToken token)
        {
            if (tRepository.findById(token.id) != null)
            {
                tRepository.update(token);
            }
            else
            {
                throw new ArgumentException("Объект с таким id не найден");
            }
        }

        public void save(RefToken token)
        {
            if (tRepository.findById(token.id) == null)
            {
                tRepository.save(token);
            }
            else
            {
                throw new ArgumentException("Объект с таким id уже существует");
            }
        }

        public void deleteById(long id)
        {
            if (tRepository.findById(id) != null)
            {
                tRepository.deleteById(id);
            }
            else
            {
                throw new ArgumentException("Объект с таким id не найден");
            }
        }
    }
}
