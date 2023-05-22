using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class ClinicServiceRepository : IClinicServiceRepository
    {
        private DataContext db;

        public ClinicServiceRepository(DataContext db)
        {
            this.db = db;
        }

        public ClinicService findById (long id)
        {
            return db.clinicServices.Find(id);
        }

        public List<ClinicService> findAll()
        {
            return db.clinicServices.ToList();
        }

        public void save (ClinicService clinicService)
        {
            db.clinicServices.Add(clinicService);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.clinicServices.Remove(db.clinicServices.Find(id));
            db.SaveChanges();
        }

        public void update(ClinicService clinicService)
        {
            db.clinicServices.Update(clinicService);
            db.SaveChanges();
        }

    }
}
