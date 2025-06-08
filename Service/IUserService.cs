using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace Service
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public Task<PagedResult<UserDto>> GetUsersPagedAsync(int pageNumber, int pageSize, string? search = null);

        public UserDto GetUser(int id);

        public bool AddUser(UserDto userDto);

        public bool UpdateUser(UserDto userDto);

        public bool DeleteUser(int id);

        public UserDto Login(string email, string password);

        public bool RegisterUser(UserDto userDto);

        public bool SendConfirmationEmail(string email);
        public bool ConfirmEmail(string tokenValue);
    }
}
