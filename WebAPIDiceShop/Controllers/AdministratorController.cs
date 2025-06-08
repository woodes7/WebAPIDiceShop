using Microsoft.AspNetCore.Mvc;
using DataModel;
using Service;


namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorController : ControllerBase
    {
        private IAdministratorService administratorService;
        public AdministratorController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        [HttpGet("isAdmin/{userId}")]
        public bool IsAdmin(int userId)
        {
            return administratorService.IsAdmin(userId);
        }


    }
}
