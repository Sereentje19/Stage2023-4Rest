using Back_end;
using System.Linq;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

using var db = new NotificationContext();

// Note: This sample requires the database to be created before running.
// Console.WriteLine($"Database path: {db.DbPath}.");

// // Create
// Console.WriteLine("Inserting a new blog");
// db.Add(new User { Email = "ser@k.nl", Password = "12345" });
// Console.WriteLine("gelukt!");
// db.SaveChanges();

// // Read
// Console.WriteLine("Querying for a blog");
// var user = db.Users
//     .OrderBy(b => b.UserId)
//     .First();

// // Delete
// Console.WriteLine("Delete the blog");
// db.Remove(user);
// db.SaveChanges();



var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

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
