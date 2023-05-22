using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private DataContext db;

        public DoctorRepository(DataContext _db)
        {
            db = _db;

            HashSet<Appointment> appointments = db.appointments.ToHashSet();
            

            foreach (Doctor doctor in db.doctors)
            {
                doctor.appointments = new List<Appointment>(appointments.Where(a => a.doctorId == doctor.id).ToList());
            }
        }

        public Doctor findById(long id)
        {
            return db.doctors.Find(id);
        }

        public List<Doctor> findAll()
        {
            return db.doctors.ToList();
        }

        public void save(Doctor doctor)
        {
            db.doctors.Add(doctor);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.doctors.Remove(db.doctors.Find(id));
            db.SaveChanges();
        }

        public void update(Doctor doctor)
        {
            db.doctors.Update(doctor);
            db.SaveChanges();
        }
    }
}
