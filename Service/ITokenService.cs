using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITokenService
    {
        public TokenDto GenerateResetToken(string email);
        public bool ResetPasswordWithToken(string tokenValue, string newPassword);
    }
}
