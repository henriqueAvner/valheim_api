using System.Security.Claims;
using System.Text;
using api_valheim.Repository;
using Auth.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ValheimContext>();
builder.Services.AddScoped<IValheimContext, ValheimContext>();

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("4d82a63bbdc67c1e4784edd6587f3730c"))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("levelA", policy => policy.RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Role, "Admin"));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("levelB", policy => policy.RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Role, "User"));
});


// SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret), SecurityAlgorithms.HmacSha256)
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
