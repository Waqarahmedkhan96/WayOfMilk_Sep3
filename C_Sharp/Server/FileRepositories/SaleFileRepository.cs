using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class SaleFileRepository : FileRepository<Sale>, ISaleRepository
{
    public SaleFileRepository() : base(Path.Combine("Data", "sales.json")) { }

    public async Task<Sale> AddAsync(Sale sale)
    {
        var items = await LoadAsync();
        sale.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(sale);
        await SaveAsync(items);
        return sale;
    }

    public async Task UpdateAsync(Sale sale)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == sale.Id);
        if (idx < 0) throw new NotFoundException("Sale not found.");
        items[idx] = sale;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Sale not found.");
        await SaveAsync(items);
    }

    public async Task<Sale> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Sale not found.");
    }

    public IQueryable<Sale> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
