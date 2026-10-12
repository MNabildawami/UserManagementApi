using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Models;
using UserManagementApi.Services;

namespace UserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserStore _users;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserStore users, ILogger<UsersController> logger)
    {
        _users = users;
        _logger = logger;
    }

    // GET: /api/users
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<User>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<User>> GetAll()
    {
        return Ok(_users.GetAll());
    }

    // GET: /api/users/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetById(int id)
    {
        var user = _users.GetById(id);
        return user is null ? NotFound(new { message = $"User {id} was not found." }) : Ok(user);
    }

    // POST: /api/users
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Create([FromBody] UserRequest request)
    {
        if (_users.EmailExists(request.Email))
        {
            return Conflict(new { message = "A user with this email already exists." });
        }

        var user = _users.Add(request);
        _logger.LogInformation("Created user with ID {UserId}", user.Id);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // PUT: /api/users/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Update(int id, [FromBody] UserRequest request)
    {
        if (_users.GetById(id) is null)
        {
            return NotFound(new { message = $"User {id} was not found." });
        }

        if (_users.EmailExists(request.Email, exceptUserId: id))
        {
            return Conflict(new { message = "Another user already uses this email." });
        }

        var updated = _users.Update(id, request);
        if (updated is null)
        {
            return NotFound(new { message = $"User {id} was not found." });
        }

        _logger.LogInformation("Updated user with ID {UserId}", id);
        return Ok(updated);
    }

    // DELETE: /api/users/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        if (!_users.Delete(id))
        {
            return NotFound(new { message = $"User {id} was not found." });
        }

        _logger.LogInformation("Deleted user with ID {UserId}", id);
        return NoContent();
    }
}
