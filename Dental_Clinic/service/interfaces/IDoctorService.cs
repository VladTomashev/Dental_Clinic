using Dental_Clinic.entity;

namespace Dental_Clinic.service.interfaces
{
    public interface IDoctorService : IService<Doctor>
    {
        public List<DateTime> freeTime(long id, DateOnly date);
    }
}
