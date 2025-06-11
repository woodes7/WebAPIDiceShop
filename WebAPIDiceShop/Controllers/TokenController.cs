using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        // POST: api/token/generate
        [HttpPost("generateToken")]
        public IActionResult GenerateToken(string email)
        {
            var token = tokenService.GenerateResetToken(email);

            if (token == null)
                return NotFound("No se encontró ningún usuario con ese correo.");

            return Ok(new
            {
                success = true,
                message = "Se ha enviado un enlace para restablecer tu contraseña.",                
            });
        }
        [AllowAnonymous]
        // POST: api/token/reset-password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(string tokenValue, string newPassword)
        {
            Console.WriteLine(tokenValue);
            Console.WriteLine(newPassword);
           
            var result = tokenService.ResetPasswordWithToken(tokenValue, newPassword);

            if (!result)
                return BadRequest("Token inválido o expirado.");

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }
    }
}
