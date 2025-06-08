using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    internal class TokenService : ITokenService
    {

        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public TokenService(IDbContextFactory<DiceShopContext> diceShopContextFactory, IUserService userService, IEmailService emailService)
        {
            this.diceShopContextFactory = diceShopContextFactory;
            this.userService = userService;
            this.emailService = emailService;
        }

        public TokenDto GenerateResetToken(string email)
        {
            try
            {
                using var diceShopContext = diceShopContextFactory.CreateDbContext();
                var user = diceShopContext.Users.FirstOrDefault(u => u.Email == email);
                if (user == null) return null;

                var token = new Token
                {
                    UserId = user.Id,
                    TokenValue = Guid.NewGuid().ToString(),
                    Expiration = DateTime.UtcNow.AddMinutes(10)
                };

                diceShopContext.Tokens.Add(token);
                diceShopContext.SaveChanges();

                // Enlace de recuperación
                var resetLink = $"http://localhost:4200/pages/change-password?token={token.TokenValue}";
                var body = $@"
            <p>Hola {user.FullName},</p>
            <p>Has solicitado restablecer tu contraseña.</p>
            <p><a href='{resetLink}'>Haz clic aquí para cambiarla</a>.</p>
            <p>Este enlace expirará en 10 minutos.</p>";

                var emailSent = emailService.SendEmail(user.Email, "Restablece tu contraseña", body);

                if (!emailSent)
                {
                    Console.WriteLine($"[ERROR] No se pudo enviar el correo a {user.Email}");
                    return null;
                }

                return new TokenDto
                {
                    Id = token.Id,
                    UserId = token.UserId,
                    TokenValue = token.TokenValue,
                    Expiration = token.Expiration
                };
            }
            catch (Exception ex)
            {
                // Puedes registrar el error o lanzarlo según convenga
                Console.WriteLine($"[ERROR] Fallo en GenerateResetToken: {ex.Message}");
                return null;
            }
        }


        public bool ResetPasswordWithToken(string tokenValue, string newPassword)
        {
            try
            {
                using var diceShopContext = diceShopContextFactory.CreateDbContext();
                // Paso 1: Buscar el token por su valor
                var token = diceShopContext.Tokens.FirstOrDefault(t => t.TokenValue == tokenValue);
                if (token == null)
                {
                    Console.WriteLine("[ERROR] Token no encontrado.");
                    return false;
                }

                // Paso 2: Comprobar si está expirado
                if (token.Expiration <= DateTime.UtcNow)
                {
                    Console.WriteLine("[ERROR] Token expirado.");
                    return false;
                }

                // Paso 3: Obtener el usuario a partir del UserId del token
                var user = diceShopContext.Users.FirstOrDefault(u => u.Id == token.UserId);
                if (user == null)
                {
                    Console.WriteLine("[ERROR] Usuario no encontrado.");
                    return false;
                }

                // Paso 4: Actualizar la contraseña del usuario
                var userDto = user.Adapt<UserDto>();
                userDto.Password = newPassword;

                var result = userService.UpdateUser(userDto);

                if (result)
                {
                    // Paso 5: Eliminar el token tras su uso
                    diceShopContext.Tokens.Remove(token);
                    diceShopContext.SaveChanges();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Fallo en ResetPasswordWithToken: {ex.Message}");
                return false;
            }
        }


    }
}
