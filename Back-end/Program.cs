using Back_end;
using System.Linq;
using Back_end.Models;

using var db = new NotificationContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new User { Email = "ser@k.nl", Password = "12345" });
Console.WriteLine("gelukt!");
db.SaveChanges();

// Read
Console.WriteLine("Querying for a blog");
var user = db.Users
    .OrderBy(b => b.UserId)
    .First();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(user);
db.SaveChanges();



// using Back_end;
// using Back_end.Models;

// var host = CreateHostBuilder(args).Build();

// CreateDbIfNotExists(host);

// host.Run();

// static void CreateDbIfNotExists(IHost host)
// {
//     using (var scope = host.Services.CreateScope())
//     {
//         var services = scope.ServiceProvider;
//         try
//         {
//             var context = services.GetRequiredService<NotificationContext>();
//             DbInitializer.Initialize(context);
//         }
//         catch (Exception ex)
//         {
//             var logger = services.GetRequiredService<ILogger<Program>>();
//             logger.LogError(ex, "An error occurred creating the DB.");
//         }
//     }
// }

// static IHostBuilder CreateHostBuilder(string[] args) =>
//     Host.CreateDefaultBuilder(args)
//         .ConfigureWebHostDefaults(webBuilder =>
//         {
//             webBuilder.UseStartup<Startup>();
//         });



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Set the URL and port
app.Urls.Add("http://localhost:5080");

app.Run();
