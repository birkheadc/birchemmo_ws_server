using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BirchemmoWsServer.Game;
using BirchemmoWsServer.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IWorldManager, WorldManager>();
builder.Services.AddSingleton<IPlayersManager, PlayersManager>();
builder.Services.AddSingleton<IPawnsManager, PawnsManager>();
builder.Services.AddSingleton<IGameManager, GameManager>();

// builder.Services.AddAuthentication(options =>
// {
//   options.DefaultAuthenticateScheme = "SignalRAuthentication";
//   options.DefaultChallengeScheme = "SignalRAuthentication";
// }).AddScheme<SignalRAuthenticationOptions, SignalRAuthenticationHandler>("SignalRAuthentication", options => {});

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.TokenValidationParameters = new()
  {
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = false,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecretsecretsecretsecret")),
    ValidateIssuerSigningKey = true
  };
  
  options.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      var accessToken = context.Request.Query["access_token"];
      var path = context.HttpContext.Request.Path;
      if (!string.IsNullOrEmpty(accessToken) &&
          (path.StartsWithSegments("/game")))
      {
          context.Token = accessToken;
      }
      return Task.CompletedTask;
    }
  };
});

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy(name: "RequireValidatedUser", policy => policy.AddRequirements(new RequireValidatedUserRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, RequireValidatedUserHandler>();

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

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("All");

app.MapHub<GameHub>("/game");

// Create a token

// User user = new User(Guid.NewGuid(), Role.SUPER_ADMIN);
// List<Claim> claims = new();
// claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
// claims.Add(new Claim(ClaimTypes.Name, "super"));
// claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

// SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("secretsecretsecretsecretsecretsecret"));
// SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

// SecurityTokenDescriptor descriptor = new()
// {
//   Subject = new ClaimsIdentity(claims),
//   Expires = DateTime.UtcNow.AddDays(1),
//   SigningCredentials = credentials
// };

// JwtSecurityTokenHandler handler = new();

// var token = handler.CreateToken(descriptor);
// Console.WriteLine("Token: ");
// Console.WriteLine(handler.WriteToken(token));
//

app.Run();