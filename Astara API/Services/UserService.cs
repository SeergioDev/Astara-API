using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataAccess.mytasks.Model;
using Astara_API.DataModel;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Astara_API.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationResponse _authenticationResponse;
        private readonly JWT _jwt;
        private readonly myTasksContext _context;

        public UserService(IDbContextFactory<myTasksContext> context, IOptions<JWT> jwt)
        {
            _authenticationResponse = new AuthenticationResponse();
            _context = context.CreateDbContext();
            _jwt = jwt.Value;
        }

        public AuthenticationResponse getToken(string user, string pass)
        {

            var userResult = getUser(user, pass);

            if (userResult != null)
            {
                _authenticationResponse.Token = generateToken(userResult);
            }

            return _authenticationResponse;
        }

        private User getUser(string user, string pass)
        {
            var userResult = _context.Users.Where(u => u.Usuario == user && u.Password == pass).FirstOrDefault();

            if (userResult != null) { return userResult; } else return null;

        }

        private IEnumerable<Claim> getTokenClaims(User user)
        {
            return new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Surname, user.Apellidos),
                new Claim(ClaimTypes.Role, user.Rol.ToString())
            };
        }

        private string generateToken(User user)
        {
            if (user != null)
            {
                //Generamos token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwt.Secret);
                var tokenDescr = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddSeconds(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Subject = new ClaimsIdentity(getTokenClaims(user))
                };
                var token = tokenHandler.CreateToken(tokenDescr);

                return tokenHandler.WriteToken(token);
            }

            return String.Empty;

        }

        public List<User> getUsers()
        {

            return _context.Users.ToList();

        }

    }
}
