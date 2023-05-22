using System.Text.Json.Serialization;

namespace Dental_Clinic.entity
{
    public class Doctor : AbstractEntity
    {
        public string FIO { get; set; }
        public string position { get; set; }

        [JsonIgnore]
        public List<Appointment>? appointments { get; set; }

    }
}
