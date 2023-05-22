using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;

namespace Dental_Clinic.service
{
    public class DoctorService : IDoctorService
    {
        private IDoctorRepository repository;

        public DoctorService(IDoctorRepository repository)
        {
            this.repository = repository;
        }

        public Doctor read(long id)
        {
            return repository.findById(id);
        }

        public List<Doctor> read()
        {
            return repository.findAll();
        }

        public void save(Doctor doctor)
        {
            if (repository.findById(doctor.id) == null)
            {
                repository.save(doctor);
            }
            else
            {
                throw new ArgumentException("Объекст с таким id уже существует");
            }
        }

        public void delete(long id)
        {
            if (repository.findById(id) != null)
            {
                repository.deleteById(id);
            }
            else
            {
                throw new ArgumentException("Объект с таким id не найден");
            }
        }

        public void update(Doctor _doctor)
        {
            Doctor doctor = repository.findById(_doctor.id);
            if (doctor == null) throw new ArgumentException("Объект с таким id не найден");
            doctor.FIO = _doctor.FIO;
            doctor.position = _doctor.position;
            repository.update(doctor);
        }

        public List<DateTime> freeTime(long id, DateOnly date)
        {
            Doctor doctor = repository.findById(id);
            if (doctor == null) throw new ArgumentException("Доктор не найден");

            if (date < DateOnly.FromDateTime(DateTime.Now) || date > DateOnly.FromDateTime(DateTime.Now).AddDays(14))
                throw new ArgumentException("Запись можно производить в течении следующих 14 дней");

            DateTime workDayBegin = new DateTime(date.Year, date.Month, date.Day, 10, 0, 0);
            DateTime workDayEnd = new DateTime(date.Year, date.Month, date.Day, 20, 0, 0);

            List<Appointment> appointments = doctor.appointments.Where(a => a.appointmentTime >= workDayBegin
            && a.appointmentTime <= workDayEnd).ToList();

            List<DateTime> freeTime = new List<DateTime>();

            int h;
            if (date == DateOnly.FromDateTime(DateTime.Now)) h = DateTime.Now.Hour + 1;
            else h = 10;
           
            for (; h <= 20; h++)
            {
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, h, 0, 0);

                if (!appointments.Any(a => a.appointmentTime == dateTime))
                {
                    freeTime.Add(dateTime);
                }
            }
            return freeTime;
        }
    }
}
