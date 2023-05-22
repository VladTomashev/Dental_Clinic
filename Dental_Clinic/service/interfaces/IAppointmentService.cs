using Dental_Clinic.entity;
using Dental_Clinic.entity.models;

namespace Dental_Clinic.service.interfaces
{
    public interface IAppointmentService : IService<Appointment>
    {
        public void make(long doctorId, long patientId, long serviceId, DateTime date);
    }
}
