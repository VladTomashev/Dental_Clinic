namespace Dental_Clinic.entity.models
{
    public class AppointmentResponse
    {
        public int totalCost { get; set; }
        public List<Appointment> appointments { get; set; }
    }
}
