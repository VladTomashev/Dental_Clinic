using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic.entity.models
{
    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
