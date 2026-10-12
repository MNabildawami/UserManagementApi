using UserManagementApi.Middleware;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register application services and controller support.
builder.Services.AddControllers();
builder.Services.AddSingleton<IUserStore, InMemoryUserStore>();

var app = builder.Build();

// Keep exception handling outside the logging middleware so errors are returned consistently
// and the request logger can record the final response status.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
