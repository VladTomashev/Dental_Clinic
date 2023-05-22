using Dental_Clinic.entity.models;

namespace Dental_Clinic.service.interfaces
{
    public interface IUserService
    {
        TokenModel Login(LoginModel model);
        void Registration(RegModel model);
        void AddDoctor(DoctorRegModel model);
        void DeleteDoctorById(long id);

    }
}
