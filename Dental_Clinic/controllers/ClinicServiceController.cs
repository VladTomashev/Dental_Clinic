using Dental_Clinic.entity;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClinicServiceController : Controller
    {
        private IClinicServiceService service;

        public ClinicServiceController(IClinicServiceService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public IActionResult getById(long id)
        {
            ClinicService clinicService = service.read(id);
            if (clinicService == null) return NotFound();
            return Json(clinicService);
        }

        [HttpGet]
        public IActionResult getAll()
        {
            List<ClinicService> clinicServices = service.read();
            if (!clinicServices.Any()) return NotFound();
            return Json(clinicServices);
        }

        [HttpPost, Authorize(Roles = "manager")]
        public IActionResult post(ClinicService clinicService)
        {
            try
            {
                service.save(clinicService);
                return Ok("Услуга успешно добавлена");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut, Authorize(Roles = "manager")]
        public IActionResult put(ClinicService clinicService)
        {
            try
            {
                service.update(clinicService);
                return Ok("Услуга успешно обновлена");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "manager")]
        public IActionResult delete(long id) 
        {
            try
            {
                service.delete(id);
                return Ok("Услуга успешно удалена");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
