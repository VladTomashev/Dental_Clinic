using Dental_Clinic.entity;
using Dental_Clinic.entity.models;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private IPatientService pService;

        public PatientController(IPatientService pService)
        {
            this.pService = pService;
        }

        [HttpGet, Authorize(Roles = "manager")]
        public IActionResult getAll()
        {
            List<Patient> patients = pService.read();
            if (!patients.Any()) return NotFound();
            return Json(patients);
        }

        [HttpGet("{id}"), Authorize(Roles = "manager")]
        public IActionResult getById(long id)
        {
            Patient patient = pService.read(id);
            if (patient == null) return NotFound();
            return Json(patient);
        }

        [HttpGet("getMyProfile"), Authorize(Roles = "patient")]
        public IActionResult getMyProfile()
        {
            var i = User.FindFirst("userEntityId");
            long myId = Convert.ToInt64(i.Value);

            Patient patient = pService.read(myId);
            if (patient == null) return NotFound();
            return Json(patient);
        }

        [HttpGet("getMyAppointments"), Authorize(Roles = "patient")]
        public IActionResult getMyAppointments()
        {
            var i = User.FindFirst("userEntityId");
            long myId = Convert.ToInt64(i.Value);

            Patient patient = pService.read(myId);
            if (patient == null || !patient.appointments.Any()) return NotFound();

            int totalCost = patient.appointments.Select(a => a.clinicService.price).Sum();

            return Json(new AppointmentResponse
            {
                totalCost = totalCost,
                appointments= patient.appointments
            });
        }

        [HttpPut("changeMyProfile"), Authorize(Roles = "patient")]
        public IActionResult put (string new_FIO, string new_phoneNumber)
        {
            var i = User.FindFirst("userEntityId");
            long myId = Convert.ToInt64(i.Value);

            Patient patient = new Patient();
            patient.id = myId;
            patient.FIO = new_FIO;
            patient.phoneNumber = new_phoneNumber;

            try
            {
                pService.update(patient);
                return Ok("Профиль успешно обновлён");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
