using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;

namespace Dental_Clinic.service
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository repository;
        private IPatientRepository pRepository;
        private IDoctorRepository dRepository;
        private IClinicServiceRepository cRepository;

        public AppointmentService(IAppointmentRepository repository, IPatientRepository pRepository, IDoctorRepository dRepository, IClinicServiceRepository cRepository)
        {
            this.repository = repository;
            this.pRepository = pRepository;
            this.dRepository = dRepository;
            this.cRepository = cRepository;
        }

        public Appointment read(long id)
        {
            return repository.findById(id);
        }

        public List<Appointment> read()
        {
            return repository.findAll();
        }

        public void make(long doctorId, long patientId, long serviceId, DateTime date)
        {
            Doctor doctor = dRepository.findById(doctorId);
            if (doctor == null) throw new ArgumentException("Доктор не найден");

            Patient patient = pRepository.findById(patientId);
            if (patient == null) throw new ArgumentException("Пациент не найден");

            ClinicService clinicService = cRepository.findById(serviceId);
            if (clinicService == null) throw new ArgumentException("Услуга не найдена");

            long appointmentId;

            if (read().Any()) appointmentId = read().Max(a => a.id) + 1;
            else appointmentId = 1;

            Appointment appointment = new Appointment
            {
                id = appointmentId,
                appointmentTime = date,
                patientId = patientId,
                doctorId = doctorId,
                serviceId = serviceId,

                patient = patient,
                doctor = doctor,
                clinicService = clinicService
            };

            repository.save(appointment);
        }

        public void save(Appointment appointment)
        {
            if (repository.findById(appointment.id) == null)
            {
                repository.save(appointment);
            }
            else
            {
                throw new ArgumentException("Объект с таким id уже существует");
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

        public void update(Appointment _appointment)
        {
            Appointment appointment = repository.findById(_appointment.id);
            if (appointment == null) throw new ArgumentException("Объект с таким id не найден");
            appointment.appointmentTime = _appointment.appointmentTime;
            repository.update(appointment);
        }
    }
}
