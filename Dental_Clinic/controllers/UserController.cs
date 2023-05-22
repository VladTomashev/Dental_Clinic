using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserService uService;
        public UserController(IUserService uService)
        {
            this.uService = uService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                return Ok(uService.Login(loginModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registration")]
        public IActionResult Registration(RegModel regModel)
        { 
            try
            {
                uService.Registration(regModel);
                return Ok("Успешная регистрация. Данные можно использовать для входа");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addDoctor"), Authorize(Roles = "manager")]
        public IActionResult AddDoctor(DoctorRegModel model)
        {
            try
            {
                uService.AddDoctor(model);
                return Ok("Учётная запись доктора добавлена. Данные можно использовать для входа");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteDoctor"), Authorize(Roles = "manager")]
        public IActionResult DeleteDoctor(long id)
        {
            try
            {
                uService.DeleteDoctorById(id);
                return Ok("Учётная запись доктора успешно удалена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
