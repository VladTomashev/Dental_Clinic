namespace Dental_Clinic.repository.interfaces
{
    public interface IRepository <T> where T : class
    {
        T findById(long id);
        List<T> findAll();
        void save(T entity);
        void deleteById(long id);
        void update(T entity);
    }
}
