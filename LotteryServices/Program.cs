using Microsoft.EntityFrameworkCore;
using LotteryServices.Interfaces;
using LotteryServices.Services;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using LotteryServices.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // Add this line to include controllers

builder.Services.AddDbContext<LoteriaDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("LotteryConString")));

// Register the services
builder.Services.AddScoped<IUsuario, ServiceUsuario>();
builder.Services.AddScoped<ILogin, ServiceLogin>();
builder.Services.AddScoped<IRol, ServiceRol>();
builder.Services.AddScoped<IPermiso, ServicePermiso>();
builder.Services.AddScoped<IModule, ServiceModule>();


builder.Services.AddSingleton<Utilidades>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = true;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("NewPolicy");
app.UseAuthentication();
app.UseAuthorization();

/**********************************************/
//TokenContext debugger
/**********************************************/

//app.Use(async (context, next) =>
//{
//    var token = context.Request.Headers["Authorization"].ToString();
//    Console.WriteLine("Token recibido: " + token); // O usa un logger para registrar el token

//    // Verifica si el token está vacío o tiene problemas
//    if (string.IsNullOrEmpty(token))
//    {
//        Console.WriteLine("No se recibió token.");
//    }
//    else if (!token.StartsWith("Bearer "))
//    {
//        Console.WriteLine("Token no tiene el formato Bearer.");
//    }

//    await next();
//});



app.MapControllers(); // Map controller routes

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
//app.Run("https://localhost:5001");

