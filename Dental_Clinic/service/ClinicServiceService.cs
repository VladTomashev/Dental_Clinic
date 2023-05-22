using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;

namespace Dental_Clinic.service
{
    public class ClinicServiceService : IClinicServiceService
    {
        private IClinicServiceRepository repository;

        public ClinicServiceService(IClinicServiceRepository repository)
        {
            this.repository = repository;
        }

        public ClinicService read (long id)
        {
            return repository.findById(id);
        }

        public List<ClinicService> read()
        {
            return repository.findAll();
        }

        public void save(ClinicService clinicService)
        {
            if (repository.findById(clinicService.id) == null)
            {
                repository.save(clinicService);
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

        public void update(ClinicService _clinicService)
        {
            ClinicService clinicService = repository.findById(_clinicService.id);
            if (clinicService == null) throw new ArgumentException("Объект с таким id не найден");
            clinicService.name = _clinicService.name;
            clinicService.price = _clinicService.price;
            repository.update(clinicService);
        }
    }
}
