using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class CustomerFileRepository : FileRepository<Customer>, ICustomerRepository
{
    public CustomerFileRepository() : base(Path.Combine("Data", "customers.json")) { }

    public async Task<Customer> AddAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.CompanyName))
            throw new ValidationException("CompanyName cannot be empty.");

        var items = await LoadAsync();
        customer.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(customer);
        await SaveAsync(items);
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == customer.Id);
        if (idx < 0) throw new NotFoundException("Customer not found.");
        items[idx] = customer;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Customer not found.");
        await SaveAsync(items);
    }

    public async Task<Customer> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Customer not found.");
    }

    public IQueryable<Customer> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
