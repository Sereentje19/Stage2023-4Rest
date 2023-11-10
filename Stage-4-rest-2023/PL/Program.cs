using Stage4rest2023.Models;
using Stage4rest2023.Repositories;
using Microsoft.EntityFrameworkCore;
using Stage4rest2023.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DbContext = Stage4rest2023.Models.DbContext;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


AddServices();
AddAuthentication();
AddCors();
ConnectionInterfaces();
AddDbConnection();
BuildApp();

//services
void AddServices()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCors();
    builder.Services.AddHttpContextAccessor();
}

//add authentication
void AddAuthentication()
{
    var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
    var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}

//cors
void AddCors()
{
    builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", b =>
    {
        b.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));
}


//db connection
void AddDbConnection()
{
    builder.Services.AddDbContext<DbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            opt => opt.EnableRetryOnFailure()));
}

//connect interfaces
void ConnectionInterfaces()
{
    builder.Services.AddScoped<IDocumentService, DocumentService>();
    builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
    
    builder.Services.AddScoped<ILoginService, LoginService>();
    builder.Services.AddScoped<ILoginRepository, LoginRepository>();
    
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    
    builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
    builder.Services.AddScoped<ILoanHistoryService, LoanHistoryService>();
    
    builder.Services.AddScoped<IMailService, MailService>();
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
    
    builder.Services.AddScoped<IJwtValidationService, JwtValidationService>();
    builder.Services.AddHostedService<DocumentExpirationCheckService>();
}


void BuildApp()
{
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("ApiCorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

    // Set the URL and port
    app.Urls.Add("http://localhost:5050");
    app.Run();
}