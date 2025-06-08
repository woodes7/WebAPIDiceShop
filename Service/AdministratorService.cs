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
    internal class AdministratorService : IAdministratorService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public AdministratorService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool IsAdmin(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Administrators.Where(a => a.UserId == userId).ToList().Count > 0;
        }

    }
}