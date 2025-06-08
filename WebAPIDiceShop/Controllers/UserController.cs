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

        [HttpGet("getUserByEmail")]
        public UserDto getUserByEmail(string email)
        {
            return this.userService.GetUserByEmail(email);
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
        public bool SendConfirmationEmail([FromQuery] string email)
        {
            try
            {
                var success = userService.SendConfirmationEmail(email);

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error en send-confirmation: {ex.Message}");
                return false;
            }
        }


        [HttpPost("confirm-email")]
        public bool ConfirmEmail(string token)
        {
            var success = userService.ConfirmEmail(token);


            return success;
        }

    }
}

