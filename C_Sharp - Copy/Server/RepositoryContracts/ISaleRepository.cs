using Entities;

namespace RepositoryContracts;

public interface ISaleRepository
{
    Task<Sale> AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(int id);
    Task<Sale> GetSingleAsync(int id);
    IQueryable<Sale> GetManyAsync();
}
