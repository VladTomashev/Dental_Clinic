using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic.entity.models
{
    public class DoctorRegModel
    {
        [Required(ErrorMessage = "Login Required")]
        public string login { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string password { get; set; }
        [Required(ErrorMessage = "FIO Required")]
        public string FIO { get; set; }
        [Required(ErrorMessage = "Position Required")]
        public string position { get; set; }
    }
}
