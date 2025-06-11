using Autofac.Extensions.DependencyInjection;
using Autofac;
using Data;
using Microsoft.EntityFrameworkCore;
using WebAPIDiceShop.Utils;
using Mapster;
using Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//TypeAdapterConfig.GlobalSettings.Scan(typeof(ProductRegister).Assembly);
// Obtener la cadena de conexión de la base de datos desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "WebAPIDiceShop",
            ValidAudience = "FrontDiceShop",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890123456789012"))
        };
    });

// Registrar DbContext
builder.Services.AddDbContextFactory<DiceShopContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Configurar Autofac como contenedor de dependencias
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((container) =>
    {
        container.RegisterModule(new AppModule());
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://pablorg.xyz") // cambia por tu dominio
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

