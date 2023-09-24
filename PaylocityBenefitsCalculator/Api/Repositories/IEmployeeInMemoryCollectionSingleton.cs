using Api.Models;

namespace Api.Repositories
{
    public interface IEmployeeInMemoryCollectionSingleton
    {
        List<Employee> Employees { get; }
    }
}
