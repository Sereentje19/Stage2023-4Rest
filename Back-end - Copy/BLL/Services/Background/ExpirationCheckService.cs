using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BLL.Services.Background
{
    public class ExpirationCheckService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        private const int WeeksBeforeExpiry5 = 5;
        private const int WeeksBeforeExpiry6 = 6;
        private const int Week = 7;
        private const int DaysBeforeDelete = 90;

        public ExpirationCheckService(IServiceProvider provider)
        {
            _provider = provider;
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
                using (IServiceScope scope = _provider.CreateScope())
                {
                    await ProcessExpiringDocumentsAsync(scope.ServiceProvider);
                    await ProcessDeletedProductsAsync(scope.ServiceProvider);
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
        private static async Task ProcessExpiringDocumentsAsync(IServiceProvider serviceProvider)
        {
            ApplicationDbContext applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            IMailService mailService = serviceProvider.GetRequiredService<IMailService>();

            DateTime targetDate6Weeks = DateTime.Now.AddDays(WeeksBeforeExpiry5 * Week);
            DateTime targetDate5Weeks = DateTime.Now.AddDays(WeeksBeforeExpiry5 * Week);

            List<Document> expiringDocuments = await applicationDbContext.Documents
                .Include(d => d.Employee)
                .Where(d => d.Date.Date == targetDate5Weeks.Date || d.Date.Date == targetDate6Weeks.Date)
                .ToListAsync();

            foreach (Document document in expiringDocuments)
            {
                int weeks = (document.Date.Date == targetDate5Weeks.Date) ? WeeksBeforeExpiry5 : WeeksBeforeExpiry6;
                Employee employee =
                    await applicationDbContext.Employees.FirstOrDefaultAsync(c =>
                        c.EmployeeId == document.Employee.EmployeeId);

                string bodyEmail = $"Het volgende document zal over {weeks} weken komen te vervallen:" +
                                   $"\nNaam: {employee.Name}" +
                                   $"\nVerloop datum: {document.Date:dd-MM-yyyy}" +
                                   $"\nType document: {document.Type.ToString()!.Replace("_", " ")}\n\n";

                mailService.SendDocumentExpirationEmail(bodyEmail, document.FileType, document.File,
                    "Document vervalt Binnenkort!");
            }
        }

        /// <summary>
        /// Asynchronously processes and permanently deletes products that have been marked as deleted.
        /// </summary>
        /// <param name="serviceProvider">The service provider for accessing the required services.</param>
        private static async Task ProcessDeletedProductsAsync(IServiceProvider serviceProvider)
        {
            ApplicationDbContext applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            DateTime targetDateToDelete = DateTime.Today.AddDays(-DaysBeforeDelete);

            List<Product> deletedProducts = await applicationDbContext.Products
                .Where(p => p.IsDeleted && p.TimeDeleted == targetDateToDelete)
                .ToListAsync();

            foreach (Product product in deletedProducts)
            {
                applicationDbContext.Products.Remove(product);
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}