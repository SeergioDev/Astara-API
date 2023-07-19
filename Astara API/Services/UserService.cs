using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataAccess.mytasks.Model;
using Astara_API.DataModel;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Astara_API.Services
{
    public class UserService : IUserService
    {
        myTasksContext _context;
        private readonly AuthenticationResponse _authenticationResponse;
        public UserService(IDbContextFactory<myTasksContext> context)
        {
            _context = context.CreateDbContext();
            _authenticationResponse = new AuthenticationResponse();
        }

        //public UserService()
        //{
        //    //_context = context.CreateDbContext();
        //    _authenticationResponse = new AuthenticationResponse();
        //}

        public AuthenticationResponse getToken(string user, string pass)
        {

            var result = _context.Users.ToList();
            //var result = _context.Users.Where(u => u.Usuario == user && u.Password == pass).FirstOrDefault();


            _authenticationResponse.User = user;
            _authenticationResponse.Password = pass;

            if (result != null)
            {
                var tokenHand = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("AstaraTest");
                var tokenDescr = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHand.CreateToken(tokenDescr);

                _authenticationResponse.Token = token.ToString();
            }

            return _authenticationResponse;
        }

    }
}
