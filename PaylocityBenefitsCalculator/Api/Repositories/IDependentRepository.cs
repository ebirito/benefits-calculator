using Api.Models;

namespace Api.Repositories
{
    public interface IDependentRepository
    {
        IEnumerable<Dependent> GetAll();
        Dependent? GetById(int id);
    }
}
