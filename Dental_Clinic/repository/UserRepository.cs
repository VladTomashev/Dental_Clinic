using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext db;

        public UserRepository(DataContext db)
        {
            this.db = db;
        }

        public User findById(long id)
        {
            return db.users.Find(id);
        }

        public List<User> findAll()
        {
            return db.users.ToList();
        }

        public void save(User user)
        {
            db.users.Add(user);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.users.Remove(findById(id));
            db.SaveChanges();
        }

        public void update(User user)
        {
            db.users.Update(user);
            db.SaveChanges();
        }
    }
}
