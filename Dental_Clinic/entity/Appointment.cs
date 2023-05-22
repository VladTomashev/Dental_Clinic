using System.Text.Json.Serialization;

namespace Dental_Clinic.entity
{
    public class Appointment : AbstractEntity
    {
        public DateTime? appointmentTime { get; set; }
        public long patientId { get; set; }
        public long doctorId { get; set; }
        public long serviceId { get; set; }

        [JsonIgnore]
        public Patient? patient { get; set; }
        [JsonIgnore]
        public Doctor? doctor { get; set; }
        [JsonIgnore]
        public ClinicService? clinicService { get; set; }
    }
}
