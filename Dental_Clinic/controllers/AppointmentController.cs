using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private IAppointmentService aService;
        private IDoctorService dService;

        public AppointmentController(IAppointmentService aService, IDoctorService dService)
        {
            this.aService = aService;
            this.dService = dService;
        }

        [HttpPost("makeAppointment"), Authorize(Roles = "patient")]
        public IActionResult makeAppointment(AppointmentModel model)
        {
            DateTime date;
            try
            {
                date = new DateTime(model.year, model.month, model.day, model.hour, 0, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Неправильная дата");
            }

            List<DateTime> times;
            try
            {
                times = dService.freeTime(model.doctorId, DateOnly.FromDateTime(date));
                if (times.Any(t => t == date))
                {
                    var i = User.FindFirst("userEntityId");
                    long myId = Convert.ToInt64(i.Value);

                    aService.make(model.doctorId, myId, model.serviceId, date);
                    return Ok("Запись успешно создана");
                }
                else
                {
                    return BadRequest("Не удалось записаться на указанное время");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Authorize(Roles = "manager")]
        public IActionResult getAll()
        {
            List<Appointment> appointments = aService.read();
            if (!appointments.Any()) return NotFound();
            return Json(appointments);
        }

        [HttpDelete("{id}"), Authorize(Roles = "manager")]
        public IActionResult delete(long id)
        {
            try
            {
                aService.delete(id);
                return Ok("Запись успешно удалена");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
