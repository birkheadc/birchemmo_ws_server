using BirchemmoWsServer.Server;

var builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<ISessionTokenValidator>(
  isDevelopment ? new SessionTokenValidatorTrustAll() : new SessionTokenValidator()
);

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy(name: "Test", policy => policy.RequireAssertion(_ => true));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        policy =>
        {
            policy
              .WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

app.UseAuthorization();

app.UseCors("All");

app.MapHub<GameHub>("/game");

app.Run();