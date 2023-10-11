using Back_end.Models;
using Back_end.Repositories;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

AddServices();
// JWTAuthentication();
AddCors();
AddDBConnection();
ConnectionInterfaces();
BuildApp();

//services
void AddServices()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCors();
    builder.Services.AddAuthentication().AddJwtBearer();
    builder.Services.AddHttpContextAccessor();
}

//JWT Authentication
void JWTAuthentication()
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        // options.Authority = $"http://{builder.Configuration["Auth0:Domain"]}";
        // options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
}


//cors
void AddCors()
{
    builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
        {
            builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
        }));
}


//db connection
void AddDBConnection()
{
    builder.Services.AddDbContext<NotificationContext>(options =>
                                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                options => options.EnableRetryOnFailure()));
}

//connect interfaces
void ConnectionInterfaces()
{
    builder.Services.AddScoped<IDocumentService, DocumentService>();
    builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
    builder.Services.AddScoped<ILoginService, LoginService>();
    builder.Services.AddScoped<ILoginRepository, LoginRepository>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IJwtValidationService, JwtValidationService>();
    builder.Services.AddScoped<IFilterService, FilterService>();
    builder.Services.AddScoped<IFilterRepository, FilterRepository>();
    builder.Services.AddScoped<IMailService, MailService>();
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
    builder.Services.AddHostedService<DocumentExpirationCheckService>();
}


void BuildApp()
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("ApiCorsPolicy");
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    // Set the URL and port
    app.Urls.Add("http://localhost:5080");
    app.Run();
}