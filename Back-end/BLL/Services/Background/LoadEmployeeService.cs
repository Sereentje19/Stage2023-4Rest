using BLL.Services.Authentication;
using DAL.Data;
using DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace BLL.Services.Background;

public class LoadEmployeeService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public LoadEmployeeService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                await CheckEmployeesExist(scope.ServiceProvider);
            }

            TimeSpan delay = TimeSpan.FromDays(1);
            await Task.Delay(delay, stoppingToken);
        }
    }

    private static async Task<IEnumerable<Employee>> GetEmployeesFromGal()
    {
        try
        {
            AzureAuthProvider authenticationProvider = new AzureAuthProvider();
            GraphServiceClient graphClient = new GraphServiceClient(authenticationProvider);

            UserCollectionResponse employees = await graphClient.Users.GetAsync();
            return employees.Value.Select(e => new Employee { Name = e.DisplayName, Email = e.Mail }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetEmployeesFromGal: {ex.Message}");
            return Enumerable.Empty<Employee>();
        }
    }


    private static async Task CheckEmployeesExist(IServiceProvider serviceProvider)
    {
        ApplicationDbContext applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        IEnumerable<Employee> existingEmployees = applicationDbContext.Employees.ToList();
        IEnumerable<Employee> employeesFromGal = await GetEmployeesFromGal();

        IEnumerable<Employee> newEmployees = employeesFromGal.Except(existingEmployees, new EmployeeComparer());

        applicationDbContext.Employees.AddRange(newEmployees);
        await applicationDbContext.SaveChangesAsync();
    }
}