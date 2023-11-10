using Stage4rest2023.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DbContext = Stage4rest2023.Models.DbContext;

namespace Stage4rest2023.Services
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

                TimeSpan delay = TimeSpan.FromDays(1);
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
            DbContext dbContext = serviceProvider.GetRequiredService<DbContext>();
            var mailService = serviceProvider.GetRequiredService<IMailService>();

            DateTime targetDate6Weeks = DateTime.Now.AddDays(6 * 7);
            DateTime targetDate5Weeks = DateTime.Now.AddDays(5 * 7);

            List<Document> expiringDocuments = await dbContext.Documents
                        .Include(d => d.Employee)
                        .Where(d => d.Date.Date == targetDate5Weeks.Date || d.Date.Date == targetDate6Weeks.Date)
                        .ToListAsync();

            foreach (Document document in expiringDocuments)
            {
                int weeks = (document.Date.Date == targetDate5Weeks.Date) ? 5 : 6;
                Employee employee = await dbContext.Customers.FirstOrDefaultAsync(c => c.EmployeeId == document.Employee.EmployeeId);

                mailService.SendEmail(employee.Name, document.FileType, document.Date, document.Type, document.File, weeks);
            }
        }
    }
}
