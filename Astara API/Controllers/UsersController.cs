using Microsoft.AspNetCore.Mvc;
using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataAccess.mytasks.Model;
using Astara_API.DataModel;
using Astara_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Astara_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IDbContextFactory<myTasksContext> _context;
        private IUserService _userService;

        public UsersController(IDbContextFactory<myTasksContext> context, IUserService userService)
        {
            _context = context;
            context.CreateDbContext();
            _userService = userService;
        }

        // GET: Users
        [HttpPost]
        //public async Task<IEnumerable<User>> GetUsersAsync([FromBody] Authentication modelAuthentication)
        public async Task<IActionResult> GetUsersAsync([FromBody] Authentication modelAuthentication)
        {

            var response = _userService.getToken(modelAuthentication.User, modelAuthentication.Password);

            return response == null ? Unauthorized() : Ok(response);
        }


    }
}
