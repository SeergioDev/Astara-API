using Astara_API.DataModel;
using Astara_API.DataAccess.mytasks.Model;

namespace Astara_API.Services
{
    public interface IUserService
    {
        AuthenticationResponse getToken(string user, string pass);
        List<User> getUsers();
        void insertUsers(List<User> user);
        void updateUser(int userID, User userModel);
        void deleteUser(int userID);
    }
}
