using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class PatientRepository : IPatientRepository
    {
        private DataContext db;

        public PatientRepository(DataContext _db)
        {
            db = _db;

            HashSet<Appointment> appointments = db.appointments.ToHashSet();
            

            foreach (Patient patient in db.patients)
            {
                patient.appointments = new List<Appointment>(appointments.Where(a => a.patientId == patient.id).ToList());
            }
        }

        public Patient findById(long id)
        {
            return db.patients.Find(id);
        }

        public List<Patient> findAll()
        {
            return db.patients.ToList();
        }

        public void save (Patient patient)
        {
            db.patients.Add(patient);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.patients.Remove(db.patients.Find(id));
            db.SaveChanges();
        }

        public void update(Patient patient)
        {
            db.patients.Update(patient);
            db.SaveChanges();
        }

    }
}
