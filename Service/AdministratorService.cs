using Data;
using DataModel;
using Mapster;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class AdministratorService : IAdministratorService
    {
        private readonly DiceShopContext diceShopContext;
        public AdministratorService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }

        public bool IsAdmin(int userId)
        {
            return diceShopContext.Administrators.Where(a => a.UserId == userId).ToList().Count > 0;
        }

    }
}