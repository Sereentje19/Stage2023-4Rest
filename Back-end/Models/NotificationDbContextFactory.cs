using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Back_end.Models
{
    public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationContext>
    {
        public NotificationContext CreateDbContext(string[] args)
        {
            // protected override void OnConfiguring(DbContextOptionsBuilder options) 
            // => options.UseSqlServer("Server=localhost;Database=Automated_notifications;Integrated Security=SSPI;TrustServerCertificate=True;Encrypt=False;",
            // options => options.EnableRetryOnFailure());

            var optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Automated_notifications;Integrated Security=SSPI;TrustServerCertificate=True;Encrypt=False;",
            options => options.EnableRetryOnFailure());

            return new NotificationContext(optionsBuilder.Options);
        }
    }
}