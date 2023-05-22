using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.repository.interfaces;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dental_Clinic.service
{
    public class UserService : IUserService
    {
        private IUserRepository uRepository;
        private ITokenService tService;
        private IPatientService pService;
        private IDoctorService dService;

        public UserService(IUserRepository uRepository, ITokenService tService, IPatientService pService, IDoctorService dService)
        {
            this.uRepository = uRepository;
            this.tService = tService;
            this.pService = pService;
            this.dService = dService;
        }

        public TokenModel Login(LoginModel model)
        {
            User user = uRepository.findAll().FirstOrDefault(u => (u.login == model.login) && (u.password == model.password));
            if (user == null) throw new ArgumentException("Неверный логин или пароль");

            var claims = new List<Claim>();

            if (user.role == "manager")
            {
                claims.Add(new Claim(ClaimTypes.Role, user.role));
                claims.Add(new Claim(ClaimTypes.Name, user.login));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, user.role));
                claims.Add(new Claim(ClaimTypes.Name, user.login));
                claims.Add(new Claim("userEntityId", user.userEntityId.ToString()));
            }

            var accessToken = tService.GenerateAccessToken(claims);
            var refreshTokenString = tService.GenerateRefreshToken();

            if (user.refreshToken != null)
            {
                user.refreshToken.token = refreshTokenString;
                user.refreshToken.lifeTime = DateTime.Now.AddDays(7);
                tService.update(user.refreshToken);
            }
            else
            {
                RefToken refToken = new RefToken(user.id, refreshTokenString, DateTime.Now.AddDays(7), user);
                user.refreshToken = refToken;
                tService.save(refToken);
            }

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString
            };
        }

        public void Registration(RegModel model)
        {
            if (uRepository.findAll().Any(u => u.login == model.login)) throw new ArgumentException("Логин уже занят");

            Patient patient = new Patient();
            patient.id = pService.read().Any() ? pService.read().Max(p => p.id) + 1 : 1;
            patient.FIO = model.FIO;
            patient.phoneNumber = model.phoneNumber;
            patient.appointments = new List<Appointment>();
            pService.save(patient);

            User user = new User();
            user.id = uRepository.findAll().Any() ? uRepository.findAll().Max(u => u.id) + 1 : 1;
            user.login = model.login;
            user.password = model.password;
            user.role = "patient";
            user.userEntityId = patient.id;

            uRepository.save(user);
        }

        public void AddDoctor(DoctorRegModel model)
        {
            if (uRepository.findAll().Any(u => u.login == model.login)) throw new ArgumentException("Логин уже занят");

            Doctor doctor = new Doctor();
            doctor.id = dService.read().Any() ? dService.read().Max(d => d.id) + 1 : 1;
            doctor.FIO = model.FIO;
            doctor.position = model.position;
            doctor.appointments = new List<Appointment>();
            dService.save(doctor);

            User user = new User();
            user.id = uRepository.findAll().Any() ? uRepository.findAll().Max(u => u.id) + 1 : 1;
            user.login = model.login;
            user.password = model.password;
            user.role = "doctor";
            user.userEntityId = doctor.id;

            uRepository.save(user);
        }

        public void DeleteDoctorById(long id)
        {
            Doctor doctor = dService.read(id);
            User user = uRepository.findAll().FirstOrDefault(u => u.role == "doctor" && u.userEntityId == id);

            if (doctor == null || user == null) throw new ArgumentException("Неверный id");

            dService.delete(id);
            if (user.refreshToken != null) tService.deleteById(user.refreshToken.id);
            uRepository.deleteById(user.id);
        }

    }
}
