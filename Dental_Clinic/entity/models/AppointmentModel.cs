using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic.entity.models
{
    public class AppointmentModel
    {
        public long doctorId { get; set; }
        public long serviceId { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        [Range(10, 20, ErrorMessage = "Возможное время записи: с 10 до 20 включительно")]
        public int hour { get; set; }
    }
}
