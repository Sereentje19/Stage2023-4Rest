using Stage4rest2023.Models;
using Stage4rest2023.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Hosting;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

AddServices();
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


//cors
void AddCors()
{
    builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173") 
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
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
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
    builder.Services.AddScoped<ILoanHistoryService, LoanHistoryService>();
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
    app.Urls.Add("http://localhost:5050");
    app.Run();
}