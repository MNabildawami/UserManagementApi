using UserManagementApi.Models;

namespace UserManagementApi.Services;

public interface IUserStore
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
    bool EmailExists(string email, int? exceptUserId = null);
    User Add(UserRequest request);
    User? Update(int id, UserRequest request);
    bool Delete(int id);
}
