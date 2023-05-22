using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private DataContext db;

        public AppointmentRepository(DataContext _db)
        {
            db = _db;

            HashSet<Patient> patients = db.patients.ToHashSet();
            HashSet<Doctor> doctors = db.doctors.ToHashSet();
            HashSet<ClinicService> clinicServices = db.clinicServices.ToHashSet();

            foreach (Appointment appointment in db.appointments)
            {
                appointment.patient = patients.Where(p => p.id == appointment.patientId).FirstOrDefault();
                appointment.doctor = doctors.Where(d => d.id == appointment.doctorId).FirstOrDefault();
                appointment.clinicService = clinicServices.Where(c => c.id == appointment.serviceId).FirstOrDefault();
            }
        }

        public Appointment findById(long id)
        {
            return db.appointments.Find(id);
        }

        public List<Appointment> findAll()
        {
            return db.appointments.ToList();
        }

        public void save(Appointment appointment)
        {
            db.appointments.Add(appointment);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.appointments.Remove(db.appointments.Find(id));
            db.SaveChanges();
        }

        public void update(Appointment appointment)
        {
            db.appointments.Update(appointment);
            db.SaveChanges();
        }

    }
}
