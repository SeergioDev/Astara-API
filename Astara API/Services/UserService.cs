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

            try
            {
                var userResult = getUser(user, pass);

                if (userResult != null)
                {
                    _authenticationResponse.Token = generateToken(userResult);
                }
            }
            catch
            {
                return _authenticationResponse;
            }

            return _authenticationResponse;

        }

        private User getUser(string user, string pass)
        {
            try
            {

                return _context.Users.Where(u => u.Usuario == user && u.Password == pass).FirstOrDefault();

            }
            catch
            {
                return null;
            }

        }

        private User getUser(int id)
        {
            try
            {

                return _context.Users.Where(u => u.Id == id).FirstOrDefault();

            }
            catch
            {
                return null;
            }

        }

        public void insertUsers(List<User> user)
        {
            try
            {
                foreach (var users in user)
                {
                    _context.Add(users);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar usuario", ex);
            }

        }

        public void updateUser(int userID, User userModel)
        {
            try
            {
                var userResult = getUser(userID);

                userResult.Nombre = userModel.Nombre;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar usuario", ex);
            }

        }

        public void deleteUser(int userID)
        {
            try
            {
                var userResult = _context.Users.Where(u => u.Id == userID).FirstOrDefault();

                _context.Remove(userResult);

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario", ex);
            }

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
                try
                {
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
                catch
                {
                    return String.Empty;
                }

            }

            return String.Empty;

        }

        public List<User> getUsers()
        {

            var result =  _context.Users.ToList();
            return result;

        }

    }
}
