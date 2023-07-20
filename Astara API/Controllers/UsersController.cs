using Microsoft.AspNetCore.Mvc;
using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataModel;
using Astara_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Astara_API.Controllers
{
    [ApiController]
    [Route("users")]
    [SwaggerTag("Controller Users")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly JWT _jwt;

        public UsersController(IDbContextFactory<myTasksContext> context, IUserService userService, IOptions<JWT> jwt)
        {
            _userService = userService;
            _jwt = jwt.Value;
        }

        [HttpPost]
        [Route("authenticate")]
        [SwaggerOperation(Summary = "Obtención de token", Description = "Se utiliza para realizar una llamada con X parametros para la obtención de token")]
        public async Task<IActionResult> Authenticate([FromBody] Authentication modelAuthentication)
        {

            var response = _userService.getToken(modelAuthentication.User, modelAuthentication.Password);

            return response.Token == null ? Unauthorized() : Ok(response);
        }

        //[HttpGet]
        //[Route("getUsers")]
        //public async Task<IActionResult> GetUsersAsync([FromBody] Authentication modelAuthentication)
        //{

        //    var response = _userService.getToken(modelAuthentication.User, modelAuthentication.Password);

        //    return response.Token == null ? Unauthorized() : Ok(response);
        //}


    }
}
