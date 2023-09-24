using Api.Models;

namespace Api.Repositories
{
    public class EmployeeInMemoryRepository : IEmployeeRepository
    {
        private readonly IEmployeeInMemoryCollectionSingleton _employeeInMemoryCollectionSingleton;

        public EmployeeInMemoryRepository(IEmployeeInMemoryCollectionSingleton employeeInMemoryCollectionSingleton)
        {
            _employeeInMemoryCollectionSingleton = employeeInMemoryCollectionSingleton;
        }

        public IEnumerable<Employee> GetAll()
        {
            return this._employeeInMemoryCollectionSingleton.Employees;
        }

        public Employee? GetById(int id)
        {
            return this._employeeInMemoryCollectionSingleton.Employees.FirstOrDefault(e => e.Id == id);
        }
    }
}
