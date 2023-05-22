using Dental_Clinic.entity.models;
using Dental_Clinic.service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dental_Clinic.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private ITokenService tService;

        public TokenController(ITokenService tService)
        {
            this.tService = tService;
        }

        [HttpPut("refreshToken"), Authorize]
        public IActionResult RefreshToken(TokenModel tokenModel)
        {
            try
            {
                return Json(tService.RefreshToken(tokenModel));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("revokeToken"), Authorize]
        public IActionResult RevokeToken()
        {
            try
            {
                string login = User.Identity.Name;
                tService.RevokeToken(login);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
