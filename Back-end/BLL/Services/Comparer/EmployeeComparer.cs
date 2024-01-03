using DAL.Models;

namespace BLL.Services;

public class EmployeeComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee x, Employee y)
    {
        return x.Name == y.Name && x.Email == y.Email;
    }

    public int GetHashCode(Employee obj)
    {
        return obj.Name.GetHashCode() ^ obj.Email.GetHashCode();
    }
}