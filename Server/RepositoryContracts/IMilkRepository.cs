using Entities;

namespace RepositoryContracts;

public interface IMilkRepository
{
    Task<Milk> AddAsync(Milk milk);
    Task UpdateAsync(Milk milk);
    Task DeleteAsync(int id);
    Task<Milk> GetSingleAsync(int id);
    IQueryable<Milk> GetManyAsync();
}
