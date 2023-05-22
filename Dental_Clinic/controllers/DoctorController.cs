using Dental_Clinic.entity;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        private IDoctorService dService;

        public DoctorController(IDoctorService dService)
        {
            this.dService = dService;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            List<Doctor> doctors = dService.read();
            if (!doctors.Any()) return NotFound();
            return Json(doctors);
        }

        [HttpGet("{id}")]
        public IActionResult getById(long id)
        {
            Doctor doctor = dService.read(id);
            if (doctor == null) return NotFound();
            return Json(doctor);
        }

        [HttpGet("getMyProfile"), Authorize(Roles = "doctor")]
        public IActionResult getMyProfile()
        {
            var i = User.FindFirst("userEntityId");
            long myId = Convert.ToInt64(i.Value);

            Doctor doctor = dService.read(myId);
            if (doctor == null) return NotFound();
            return Json(doctor);
        }

        [HttpGet("getMyAppointments"), Authorize(Roles = "doctor")]
        public IActionResult getMyAppointments()
        {
            var i = User.FindFirst("userEntityId");
            long myId = Convert.ToInt64(i.Value);

            Doctor doctor = dService.read(myId);
            if (doctor == null || !doctor.appointments.Any()) return NotFound();
            return Json(doctor.appointments);
        }

        [HttpGet("getFreeTime"), Authorize]
        public IActionResult getFreeTime(long doctorId, int year, int month, int day)
        {
            DateOnly dateOnly; 
            try
            {
                dateOnly = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Неправильная дата");
            }

            try
            {
                List<DateTime> times = dService.freeTime(doctorId, dateOnly);
                if (!times.Any())
                {
                    return NotFound();
                }
                else return Json(times);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("changeDoctorProfile"), Authorize(Roles = "manager")]
        public IActionResult changeDoctorProfile(Doctor doctor)
        {
            try
            {
                dService.update(doctor);
                return Ok("Доктор успешно обновлён");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
