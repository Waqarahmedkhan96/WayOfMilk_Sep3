using Entities;
using System.Linq;

namespace RepositoryContracts;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<Customer> GetSingleAsync(int id);
    IQueryable<Customer> GetManyAsync();
}
