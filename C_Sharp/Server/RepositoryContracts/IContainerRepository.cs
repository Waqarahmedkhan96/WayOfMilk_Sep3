using Entities;
using System.Linq;

namespace RepositoryContracts;

public interface IContainerRepository
{
    Task<Container> AddAsync(Container container);
    Task UpdateAsync(Container container);
    Task DeleteAsync(int id);
    Task<Container> GetSingleAsync(int id);
    IQueryable<Container> GetManyAsync();
}
