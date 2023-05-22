using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using System.Security.Claims;

namespace Dental_Clinic.service.interfaces
{
    public interface ITokenService 
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        TokenModel RefreshToken(TokenModel model);
        void RevokeToken(string login);
        void save(RefToken token);
        void deleteById(long id);
        void update(RefToken token);
    }
}
