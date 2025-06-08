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
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("users")]
        public List<UserDto> GetUsers()
        {
            return this.userService.GetUsers();
        }

        [HttpGet("usersPaged")]
        public async Task<PagedResult<UserDto>> GetUsers(int pageNumber, int pageSize, string? search = null)
        {
            return await userService.GetUsersPagedAsync(pageNumber, pageSize, search);
        }

        [HttpGet("user/{id}")]
        public UserDto GetUser(int id)
        {
            return this.userService.GetUser(id);
        }

        [HttpPost("add")]
        public bool PostUser(UserDto userDto)
        {
            var createduser = userService.AddUser(userDto);
            return createduser;
        }

        [HttpPut("edit")]
        public bool PutUser(UserDto userDto)
        {
            return userService.UpdateUser(userDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteUser(int id)
        {
            return userService.DeleteUser(id);
        }

        [HttpGet("login")]
        public ActionResult<UserDto> Login(string email, string pass)
        {
            var user = userService.Login(email, pass);

            if (user == null)
                return Unauthorized("Credenciales inválidas.");

            return Ok(user);
        }

        [HttpPost("register")]

        public ActionResult<bool> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("Datos inválidos");

            var result = userService.RegisterUser(userDto);

            if (!result)
                return BadRequest("El email ya está registrado u ocurrió un error");

            return Ok(true);
        }

        [HttpPost("send-confirmation")]
        public IActionResult SendConfirmationEmail([FromQuery] string email)
        {
            try
            {
                var success = userService.SendConfirmationEmail(email);

                if (!success)
                    return NotFound("No se encontró el usuario o ya estaba confirmado.");

                return Ok("Correo de confirmación enviado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error en send-confirmation: {ex.Message}");
                return StatusCode(500, "Error interno al intentar enviar la confirmación.");
            }
        }


        [HttpPost("confirm-email")]
        public IActionResult ConfirmEmail([FromQuery] string token)
        {
            var success = userService.ConfirmEmail(token);

            if (!success)
                return BadRequest("Token inválido o expirado.");

            return Ok("Cuenta confirmada correctamente.");
        }

    }
}

