using Back_end;
using Back_end.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Back_end
{
    public class Startup
    {



                // migrationBuilder.InsertData(
                // schema: null,
                // table: "Users",
                // columns: new[] { "UserId", "Email", "Password" },
                // values: new object[,]
                // {
                //     { 1, "Serena@Kenter.nl", "12345" },
                //     { 2, "Kerena@Senter.nl", "11111" },
                // });

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NotificationContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                            options => options.EnableRetryOnFailure()));

            services.AddControllersWithViews();
        }

        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }
        //     else
        //     {
        //         // Configure error handling for production here
        //     }

        //     // Add middleware and routing configuration here

        //     app.UseRouting();

        //     app.UseEndpoints(endpoints =>
        //     {
        //         // Configure endpoints and routes here
        //     });
        // }
    }
}
