var builder = WebApplication.CreateBuilder(args);

// Register our classifier as a singleton — one instance for the app's lifetime.
// ASP.NET will inject it into HotdogController automatically.
builder.Services.AddSingleton<HotdogClassifierService>();

// Enables the [ApiController] / [HttpPost] attribute system.
builder.Services.AddControllers();

// Allow the React dev server to call this API.
// Without this, browsers block cross-origin requests.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// UseCors must come before MapControllers — middleware order matters in ASP.NET.
app.UseCors("AllowClient");
app.MapControllers();

app.Run();
