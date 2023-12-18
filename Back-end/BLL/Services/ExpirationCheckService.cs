using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BLL.Services
{
    public class ExpirationCheckService : BackgroundService
    {
        private readonly IServiceProvider _provider;

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

            DateTime targetDate6Weeks = DateTime.Now.AddDays(6 * 7);
            DateTime targetDate5Weeks = DateTime.Now.AddDays(5 * 7);

            List<Document> expiringDocuments = await applicationDbContext.Documents
                .Include(d => d.Employee)
                .Where(d => d.Date.Date == targetDate5Weeks.Date || d.Date.Date == targetDate6Weeks.Date)
                .ToListAsync();

            foreach (Document document in expiringDocuments)
            {
                int weeks = (document.Date.Date == targetDate5Weeks.Date) ? 5 : 6;
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

        private static async Task ProcessDeletedProductsAsync(IServiceProvider serviceProvider)
        {
            ApplicationDbContext applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            DateTime targetDateToDelete = DateTime.Today.AddDays(-90);

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