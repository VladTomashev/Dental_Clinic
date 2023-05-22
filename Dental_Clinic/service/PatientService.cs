using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;

namespace Dental_Clinic.service
{
    public class PatientService : IPatientService
    {
        private IPatientRepository repository;

        public PatientService(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public Patient read(long id)
        {
            return repository.findById(id);
        }

        public List<Patient> read()
        {
            return repository.findAll();
        }

        public void save(Patient patient)
        {
            if (repository.findById(patient.id) == null)
            {
                repository.save(patient);
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

        public void update(Patient _patient)
        {
            Patient patient = repository.findById(_patient.id);
            if (patient == null) throw new ArgumentException("Оьъект с таким id не найден");
            patient.FIO = _patient.FIO;
            patient.phoneNumber = _patient.phoneNumber;
            repository.update(patient);
        }

    }
}
