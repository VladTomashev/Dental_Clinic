using Dental_Clinic.entity;
using Dental_Clinic.repository.interfaces;

namespace Dental_Clinic.repository
{
    public class TokenRepository : ITokenRepository
    {
        private DataContext db;

        public TokenRepository(DataContext db)
        {
            this.db = db;
        }

        public RefToken findById(long id)
        {
            return db.refreshTokens.Find(id);
        }

        public List<RefToken> findAll()
        {
            return db.refreshTokens.ToList();
        }

        public void save(RefToken token)
        {
            db.refreshTokens.Add(token);
            db.SaveChanges();
        }

        public void deleteById(long id)
        {
            db.refreshTokens.Remove(findById(id));
            db.SaveChanges();
        }

        public void update(RefToken token)
        {
            db.refreshTokens.Update(token);
            db.SaveChanges();
        }

    }
}
