using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic.entity.models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login Required")]
        public string login { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string password { get; set; }
    }
}
