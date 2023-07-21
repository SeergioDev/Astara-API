using Microsoft.AspNetCore.Mvc;
using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataModel;
using Astara_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Astara_API.Controllers
{
    [ApiController]
    [Route("users")]
    [SwaggerTag("Users Controller")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IDbContextFactory<myTasksContext> context, IUserService userService, IOptions<JWT> jwt)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("authenticate")]
        [SwaggerOperation(Summary = "Obtención de token", Description = "Se obtiene el token para poder realizar resto de llamadas")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin modelUserLogin)
        {

            var response = _userService.getToken(modelUserLogin.User, modelUserLogin.Password);

            return response.Token == null ? Unauthorized("Credenciales incorrectas") : Ok(response);
        }

        [HttpGet]
        [Route("users")]
        [Authorize]
        public async Task<IActionResult> GetUsersAsync()
        {

            var response = _userService.getUsers();

            return Ok(response);
        }


    }
}
