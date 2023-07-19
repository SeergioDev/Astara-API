using Astara_API.DataModel;

namespace Astara_API.Services
{
    public interface IUserService
    {
        AuthenticationResponse getToken(string user, string pass);
    }
}
