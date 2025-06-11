using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPIDiceShop.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        [HttpGet("checkUser")]
        public ActionResult CheckUser(string email)
        {
            var user =  this.userService.CheckUser(email);
            if(user!= null)
                return Ok(new
                {
                    email = user,
                    confirmed = user.EmailConfirmed // o algún DTO reducido, como user.Id, user.Email, user.Name...
                });
            else
                return Ok(new
                {
                    email = "",
                    confirmed = false // o algún DTO reducido, como user.Id, user.Email, user.Name...
                });
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

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string email, string pass)
        {
            var user = userService.Login(email, pass);

            if (user == null)
                return Unauthorized("Credenciales inválidas.");

            // Clave secreta (misma que usas en tu configuración)
            var key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Email),
            // Puedes agregar más claims si quieres
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "WebAPIDiceShop",
                Audience = "FrontDiceShop",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            // Puedes devolver solo el token o token + info usuario
            return Ok(new
            {
                token = jwt,
                user = user // o algún DTO reducido, como user.Id, user.Email, user.Name...
            });
        }

        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public bool ConfirmEmail(string token)
        {
            var success = userService.ConfirmEmail(token);


            return success;
        }

    }
}

