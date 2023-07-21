using Microsoft.AspNetCore.Mvc;
using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataAccess.mytasks.Model;
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
    [Authorize]
    [SwaggerTag("Users Controller")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IDbContextFactory<myTasksContext> context, IUserService userService, IOptions<JWT> jwt)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsersAsync()
        {

            var response = _userService.getUsers();

            return Ok(response);
        }

        [HttpPost]
        [Route("insertUsers")]
        public async Task<IActionResult> InsertUsersAsync([FromBody] List<User> modelUsers)
        {
            try
            {
                _userService.insertUsers(modelUsers);

                return StatusCode(StatusCodes.Status201Created, null);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Error al insertar usuario");
            }

        }

        [HttpPatch]
        [Route("updateUsers/{id}")]
        public async Task<IActionResult> UpdateUsersNombreAsync(int id, [FromBody] User userModel)
        {

            if (userModel != null && id > 0)
            {
                _userService.updateUser(id, userModel);

                return StatusCode(StatusCodes.Status202Accepted, null);

            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Datos incorrectos");
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUsersAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    _userService.deleteUser(id);

                    return StatusCode(StatusCodes.Status202Accepted, null);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Error al borrar usuario");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Usuario no encontrado");

        }


    }
}
