using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Services
{
    public class DocumentExpirationCheckService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        public DocumentExpirationCheckService(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationContext>();
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                    // Bereken de exacte datum die 6 weken vanaf nu is
                    var targetDate = DateTime.Now.AddDays(6 * 7);

                    // Query de database voor documenten die verlopen op de exacte datum
                    var expiringDocuments = await dbContext.Documents
                        .Where(d => d.Date.Date == targetDate.Date)
                        .ToListAsync();

                    foreach (var document in expiringDocuments)
                    {
                        // Stuur een e-mail voor elk document dat over 6 weken verloopt
                        mailService.SendEmail("customer.Name", document.Date, document.Type);
                    }
                }

                // Calculate the time until the next check
                var now = DateTime.Now;
                var nextCheckTime = now.Date.AddDays(1).AddHours(12); // Daily check at 12:00 PM
                var delay = nextCheckTime > now ? nextCheckTime - now : TimeSpan.Zero;
                await Task.Delay(delay, stoppingToken);
            }
        }


    }

}