namespace UserManagementApi.Models;

public sealed record User(
    int Id,
    string FullName,
    string Email,
    int Age,
    DateTime CreatedAtUtc);
