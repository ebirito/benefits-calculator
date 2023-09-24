using Api.Models;

namespace Api.Repositories
{
    public class DependentInMemoryRepository : IDependentRepository
    {
        private readonly IEmployeeInMemoryCollectionSingleton _employeeInMemoryCollectionSingleton;

        public DependentInMemoryRepository(IEmployeeInMemoryCollectionSingleton employeeInMemoryCollectionSingleton)
        {
            _employeeInMemoryCollectionSingleton = employeeInMemoryCollectionSingleton;
        }

        public IEnumerable<Dependent> GetAll()
        {
            return this.GetAllDependantsFromEmployeeCollection();
        }

        public Dependent? GetById(int id)
        {
            return this.GetAllDependantsFromEmployeeCollection().FirstOrDefault(x => x.Id == id);
        }

        private IEnumerable<Dependent> GetAllDependantsFromEmployeeCollection()
        {
            return this._employeeInMemoryCollectionSingleton.Employees.Select(employee => employee.Dependents).SelectMany(x => x);
        }
    }
}
