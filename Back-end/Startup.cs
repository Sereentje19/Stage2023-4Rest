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

        // private IApplicationEnvironment _appEnv;

        // public Startup(IApplicationEnvironment appEnv)
        // {
        //     _appEnv = appEnv;
        // }
        // public void ConfigureServices(IServiceCollection services)
        // {
        //     services.AddEntityFramework()
        //         .AddSqlite()
        //         .AddDbContext<MyContext>(
        //             options => { options.UseSqlite($"Data Source={_appEnv.ApplicationBasePath}/data.db"); });
        // }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NotificationContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();

            // Load configuration from appsettings.json
            // IConfiguration configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json")
            //     .Build();

            // // Use the configuration
            // services.Configure<BaseRepository>(configuration.GetSection("YourConfigurationSection"));

            // Add other services here
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure error handling for production here
            }

            // Add middleware and routing configuration here

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Configure endpoints and routes here
            });
        }
    }
}
