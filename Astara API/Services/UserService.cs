using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataAccess.mytasks.Model;
using Astara_API.DataModel;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Astara_API.Services
{
    public class UserService : IUserService
    {
        private readonly myTasksContext _context;
        private readonly AuthenticationResponse _authenticationResponse;
        private readonly JWT _jwt;

        public UserService(IDbContextFactory<myTasksContext> context, IOptions<JWT> jwt)
        {
            _context = context.CreateDbContext();
            _authenticationResponse = new AuthenticationResponse();
            _jwt = jwt.Value;
        }

        //public UserService()
        //{
        //    //_context = context.CreateDbContext();
        //    _authenticationResponse = new AuthenticationResponse();
        //}

        public AuthenticationResponse getToken(string user, string pass)
        {

            var result = _context.Users.Where(u => u.Usuario == user && u.Password == pass).FirstOrDefault();

            _authenticationResponse.User = user;
            _authenticationResponse.Password = pass;

            if (result != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwt.Secret);
                var tokenDescr = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescr);

                _authenticationResponse.Token = tokenHandler.WriteToken(token);
            }

            return _authenticationResponse;
        }

    }
}
