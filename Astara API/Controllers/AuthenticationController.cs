using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataModel;
using Astara_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Astara_API.Controllers
{
    [ApiController]
    [Route("auth")]
    [SwaggerTag("Authentication Controller")]
    public class AuthenticationController : ControllerBase
    {

        private IUserService _userService;

        public AuthenticationController(IDbContextFactory<myTasksContext> context, IUserService userService, IOptions<JWT> jwt)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        [SwaggerOperation(Summary = "Obtención de token", Description = "Se obtiene el token para poder realizar resto de llamadas")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin modelUserLogin)
        {

            var response = _userService.getToken(modelUserLogin.User, modelUserLogin.Password);

            return response.Token == null ? Unauthorized("Credenciales incorrectas") : Ok(response);
        }

    }
}
