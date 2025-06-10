using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class UserService : IUserService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        private readonly IEmailService emailService;

        public UserService(IDbContextFactory<DiceShopContext> diceShopContextFactory, IEmailService emailService)
        {                  
            this.diceShopContextFactory = diceShopContextFactory;
            this.emailService = emailService;
        }

        public bool DeleteUser(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var user = diceShopContext.Users.Find(id);
            if (user == null) return false;

            diceShopContext.Users.Remove(user);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<UserDto> GetUsers()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var usersDto = diceShopContext.Users.ProjectToType<UserDto>().ToList();
            return usersDto;
        }

        public UserDto GetUserByEmail(string email)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var usersDto = diceShopContext.Users.FirstOrDefault(u => u.Email == email).Adapt<UserDto>();
            return usersDto;
        }

        public async Task<PagedResult<UserDto>> GetUsersPagedAsync(int pageNumber, int pageSize, string? search = null)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var query = diceShopContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search) || u.Phone.Contains(search) || u.Avatar.Contains(search));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(u => u.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<UserDto>()
                .ToListAsync();

            return new PagedResult<UserDto>
            {
                TotalCount = total,
                Items = items
            };
        }

        public UserDto GetUser(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Users.Find(id).Adapt<UserDto>();
        }

        public bool UpdateUser(UserDto userDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var user = diceShopContext.Users.FirstOrDefault(c => c.Id == userDto.Id);
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            if (user == null) return false;
            userDto.Adapt(user);
            diceShopContext.Update(user);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool AddUser(UserDto userDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var userEntity = userDto.Adapt<User>();

            // Encriptar la contraseña antes de guardar
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            diceShopContext.Users.Add(userEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public UserDto Login(string email, string password)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var user = diceShopContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null) return null;

            // Verificar la contraseña con el hash guardado
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return passwordValid ? user.Adapt<UserDto>() : null;
        }

        public bool RegisterUser(UserDto userDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            // Validación mínima: que no exista otro usuario con el mismo email
            if (diceShopContext.Users.Any(u => u.Email == userDto.Email))
                return false;

            var userEntity = userDto.Adapt<User>();
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            diceShopContext.Users.Add(userEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool SendConfirmationEmail(string email)
        {
            try
            {
                using var diceShopContext = diceShopContextFactory.CreateDbContext();
                var user = diceShopContext.Users.FirstOrDefault(u => u.Email == email);
                if (user == null || user.EmailConfirmed)
                    return false;

                // Generar token y guardarlo
                var token = Guid.NewGuid().ToString();
                var tokenEntity = new Token
                {
                    TokenValue = token,
                    UserId = user.Id,
                    Expiration = DateTime.UtcNow.AddHours(24)
                };

                diceShopContext.Tokens.Add(tokenEntity);
                diceShopContext.SaveChanges();

                // Preparar y enviar email
      
                var confirmationLink = $"https://pablorg.xyz/pages/confirm-email/{token}";
                var subject = "Confirma tu cuenta";
                var body = $"Haz clic en el siguiente enlace para confirmar tu cuenta: <a href='{confirmationLink}'>Confirmar cuenta</a>";

                return emailService.SendEmail(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al enviar email de confirmación: {ex.Message}");
                return false;
            }
        }

        public bool ConfirmEmail(string tokenValue)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var token = diceShopContext.Tokens
                .Include(t => t.User)
                .FirstOrDefault(t => t.TokenValue == tokenValue);

            if (token == null || token.User == null || token.Expiration <= DateTime.UtcNow)
                return false;
 
            token.User.EmailConfirmed = true;
            diceShopContext.Users.Update(token.User);

            // Eliminar el token
            diceShopContext.Tokens.Remove(token);

            return diceShopContext.SaveChanges() > 0;
        }


    }
}