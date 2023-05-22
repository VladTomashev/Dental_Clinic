using System.Text.Json.Serialization;

namespace Dental_Clinic.entity
{
    public class Patient : AbstractEntity
    {
        public string FIO { get; set; }
        public string phoneNumber { get; set; }

        [JsonIgnore]
        public List<Appointment>? appointments { get; set; }

    }
}
