using UserManagementApi.Models;

namespace UserManagementApi.Services;

/// <summary>
/// Simple in-memory store for this learning project. Data resets when the application restarts.
/// Replace this implementation with a database-backed store for production use.
/// </summary>
public sealed class InMemoryUserStore : IUserStore
{
    private readonly object _sync = new();
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public IReadOnlyList<User> GetAll()
    {
        lock (_sync)
        {
            return _users.OrderBy(user => user.Id).ToList();
        }
    }

    public User? GetById(int id)
    {
        lock (_sync)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }
    }

    public bool EmailExists(string email, int? exceptUserId = null)
    {
        lock (_sync)
        {
            return _users.Any(user =>
                user.Id != exceptUserId &&
                string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));
        }
    }

    public User Add(UserRequest request)
    {
        lock (_sync)
        {
            var user = new User(
                _nextId++,
                request.FullName.Trim(),
                request.Email.Trim(),
                request.Age,
                DateTime.UtcNow);

            _users.Add(user);
            return user;
        }
    }

    public User? Update(int id, UserRequest request)
    {
        lock (_sync)
        {
            var index = _users.FindIndex(user => user.Id == id);
            if (index < 0)
            {
                return null;
            }

            var updated = _users[index] with
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Age = request.Age
            };

            _users[index] = updated;
            return updated;
        }
    }

    public bool Delete(int id)
    {
        lock (_sync)
        {
            var user = _users.FirstOrDefault(item => item.Id == id);
            return user is not null && _users.Remove(user);
        }
    }
}
