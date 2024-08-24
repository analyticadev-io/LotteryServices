using Microsoft.EntityFrameworkCore;
using LotteryServices.Interfaces;
using LotteryServices.Services;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using LotteryServices.Models;

var builder = WebApplication.CreateBuilder(args);


var InProduction = false;
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // Add this line to include controllers


if (InProduction)
{
    builder.Services.AddDbContext<LoteriaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LotteryConStringPROD")));
}
else
{
    builder.Services.AddDbContext<LoteriaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LotteryConString")));

}

// Register the services
builder.Services.AddScoped<IUsuario, ServiceUsuario>();
builder.Services.AddScoped<ILogin, ServiceLogin>();
builder.Services.AddScoped<IRol, ServiceRol>();
builder.Services.AddScoped<IPermiso, ServicePermiso>();
builder.Services.AddScoped<IModule, ServiceModule>();
builder.Services.AddScoped<ISorteo, ServiceSorteo>();
builder.Services.AddSingleton<Utilidades>();

builder.Services.AddSingleton<IEncriptado>(new ServiceEncriptado());

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



if (InProduction)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("NewPolicy", policy =>
        {
            policy.WithOrigins("https://analyticadev-io.github.io")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

}
else
{


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("NewPolicy", app =>
        {
            app.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();

        });
    });



}


var app = builder.Build();

if (!InProduction)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




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

app.Run();


