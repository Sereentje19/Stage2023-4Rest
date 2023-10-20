using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Services
{
    public class DocumentExpirationCheckService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DocumentExpirationCheckService> _logger;

        /// <summary>
        /// Initializes a new instance of the DocumentExpirationCheckService class with the provided dependencies.
        /// </summary>
        /// <param name="provider">The service provider for dependency injection.</param>
        /// <param name="logger">The logger used for logging.</param>
        public DocumentExpirationCheckService(IServiceProvider provider, ILogger<DocumentExpirationCheckService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        /// <summary>
        /// Executes the document expiration check logic in a background task.
        /// </summary>
        /// <param name="stoppingToken">The cancellation token to stop the background task.</param>
        /// <returns>A task representing the execution of the background task.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    try
                    {
                        await ProcessExpiringDocumentsAsync(scope.ServiceProvider);
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

        /// <summary>
        /// Processes expiring documents by checking their expiration dates and sending notifications.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <returns>A task representing the processing of expiring documents.</returns>
        private async Task ProcessExpiringDocumentsAsync(IServiceProvider serviceProvider)
        {
            NotificationContext dbContext = serviceProvider.GetRequiredService<NotificationContext>();
            var mailService = serviceProvider.GetRequiredService<IMailService>();

            var targetDate6Weeks = DateTime.Now.AddDays(6 * 7);
            var targetDate5Weeks = DateTime.Now.AddDays(5 * 7);

            var expiringDocuments = dbContext.Documents
                        .Include(d => d.Customer)
                        .Where(d => d.Date.Date == targetDate5Weeks.Date || d.Date.Date == targetDate6Weeks.Date)
                        .ToList();

            foreach (var document in expiringDocuments)
            {
                int weeks = (document.Date.Date == targetDate5Weeks.Date) ? 5 : 6;
                var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == document.Customer.CustomerId);

                mailService.SendEmail(customer.Name, document.FileType, document.Date, document.Type, document.File, weeks);
            }
        }
    }
}
