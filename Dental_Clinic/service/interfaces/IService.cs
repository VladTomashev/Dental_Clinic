using Dental_Clinic.entity;

namespace Dental_Clinic.service.interfaces
{
    public interface IService<T>
        where T : AbstractEntity
    {
        T read(long id);
        List<T> read();
        void save(T entity);
        void delete(long id);
        void update(T entity);
    }
}
