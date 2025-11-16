using Entities;

namespace RepositoryContracts;

public interface ICowRepository
{
    Task<Cow> AddAsync(Cow cow);
    Task UpdateAsync(Cow cow);
    Task DeleteAsync(int id);
    Task<Cow> GetSingleAsync(int id);
    IQueryable<Cow> GetManyAsync();
}
