using Back_end;
using Back_end.Models;
using Back_end.Repositories;
using Back_end.Services;
using Microsoft.EntityFrameworkCore;

namespace Back_end
{
    public class Startup
    {
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

            RegisterCustomDependencies(services);
        }

        private static void RegisterCustomDependencies(IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }
        //     else
        //     {
        //         app.UseHsts();
        //     }

        //     app.UseStaticFiles();
        //     app.UseFileServer();
        //     app.UseRouting();


        //     app.UseEndpoints(endpoints =>
        //     {
        //         endpoints.MapControllers();
        //     });
        // }
    }
}
