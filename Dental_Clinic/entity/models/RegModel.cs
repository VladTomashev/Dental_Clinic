using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic.entity.models
{
    public class RegModel
    {
        [Required(ErrorMessage = "Login Required")]
        public string login { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string password { get; set; }
        [Required(ErrorMessage = "FIO Required")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        public string phoneNumber { get; set; }
    }
}
