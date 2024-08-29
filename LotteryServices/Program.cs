using Microsoft.EntityFrameworkCore;
using LotteryServices.Interfaces;
using LotteryServices.Services;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Transactions;
using LotteryServices.Models;
using Hangfire;
using Hangfire.MySql;

var builder = WebApplication.CreateBuilder(args);


/**
 *---------------------- PRODUCTION VARIABLE -------------------------
 * convert to true, if you want to change to production configurations 
 * **/
var InProduction = true;

//--------------------------------------------------------------------

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // Add this line to include controllers


if (InProduction)
{
    var connectionString = builder.Configuration["LotteryConStringPROD"];

  builder.Services.AddDbContext<LoteriaDbContext>(options =>
     options.UseSqlServer(connectionString));
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
builder.Services.AddScoped<IBoleto, ServiceBoleto>();
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


/**
 * HANGFIRE
 * 
 * **/
//var sslCaRelativePath = builder.Configuration["SslSettings:SslCaPath"];
//var sslCaAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), sslCaRelativePath);
//var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection") + $"SslCa={sslCaAbsolutePath};";
var hangfireConnectionString = builder.Configuration["HANGFIRE_CONNECTION_STRING"];
var sslCaRelativePath = builder.Configuration["SslSettings:SslCaPath"];
var sslCaAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), sslCaRelativePath);
var hangfireConnectionStringWithSsl = $"{hangfireConnectionString};SslCa={sslCaAbsolutePath};";

builder.Services.AddHangfire(config =>
{
    config.UseStorage(new MySqlStorage(hangfireConnectionString, new MySqlStorageOptions
    {
        TransactionIsolationLevel = (IsolationLevel)System.Data.IsolationLevel.ReadCommitted,
        QueuePollInterval = TimeSpan.FromSeconds(15),
        JobExpirationCheckInterval = TimeSpan.FromHours(1),
        CountersAggregateInterval = TimeSpan.FromMinutes(5),
        PrepareSchemaIfNecessary = true,
        DashboardJobListLimit = 50000,
        TransactionTimeout = TimeSpan.FromMinutes(1),
        TablesPrefix = "Hangfire"
    }));
});
builder.Services.AddHangfireServer();


//--------------

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

// Configuración de Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllers(); // Map controller routes

app.Run();


