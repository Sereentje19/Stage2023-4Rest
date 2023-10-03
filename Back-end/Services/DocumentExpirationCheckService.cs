using Back_end.Models;
using Microsoft.EntityFrameworkCore;

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

        private List<Document> getDateList(NotificationContext dbContext, DateTime targetDate5Weeks, DateTime targetDate6Weeks)
        {
            var expiringDocuments6Weeks = dbContext.Documents
                .Where(d => d.Date.Date == targetDate6Weeks.Date)
                .ToList();

            var expiringDocuments5Weeks = dbContext.Documents
                .Where(d => d.Date.Date == targetDate5Weeks.Date)
                .ToList();

            var expiringDocuments = expiringDocuments6Weeks.Concat(expiringDocuments5Weeks).ToList();
            return expiringDocuments;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    NotificationContext dbContext = scope.ServiceProvider.GetRequiredService<NotificationContext>();
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                    var targetDate6Weeks = DateTime.Now.AddDays(6 * 7);
                    var targetDate5Weeks = DateTime.Now.AddDays(5 * 7);

                    try
                    {
                        List<Document> expiringDocuments = getDateList(dbContext, targetDate5Weeks, targetDate6Weeks);

                        foreach (var document in expiringDocuments)
                        {
                            int weeks = (document.Date.Date == targetDate5Weeks.Date) ? 5 : 6;
                            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == document.CustomerId);

                            mailService.SendEmail(customer.Name, document.Date, document.Type, document.Image, weeks);
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
