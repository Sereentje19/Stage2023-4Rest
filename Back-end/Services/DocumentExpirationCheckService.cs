using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Back_end.Services
{
    public class DocumentExpirationCheckService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DocumentExpirationCheckService> _logger;

        public DocumentExpirationCheckService(IServiceProvider provider, ILogger<DocumentExpirationCheckService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationContext>();
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                    // Bereken de exacte datum die 6 weken en 5 weken vanaf nu is
                    var targetDate6Weeks = DateTime.Now.AddDays(6 * 7);
                    var targetDate5Weeks = DateTime.Now.AddDays(5 * 7);

                    try
                    {
                        // Query the database for documents that expire on the exact date
                        var expiringDocuments6Weeks = dbContext.Documents
                            .Where(d => d.Date.Date == targetDate6Weeks.Date)
                            .ToList();

                        var expiringDocuments5Weeks = dbContext.Documents
                            .Where(d => d.Date.Date == targetDate5Weeks.Date)
                            .ToList();

                        // Combine the lists of documents that expire 6 weeks and 5 weeks from now
                        var expiringDocuments = expiringDocuments6Weeks.Concat(expiringDocuments5Weeks).ToList();

                        foreach (var document in expiringDocuments)
                        {
                            int weeks = (document.Date.Date == targetDate5Weeks.Date) ? 5 : 6;
                            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == document.CustomerId);

                            if (customer != null)
                            {
                                mailService.SendEmail(customer.Name, document.Date, document.Type, document.Image, weeks);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while processing document expiration check.");
                    }
                }

                var delay = TimeSpan.FromDays(1);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
