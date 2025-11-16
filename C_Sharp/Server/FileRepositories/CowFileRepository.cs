using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class CowFileRepository : FileRepository<Cow>, ICowRepository
{
    public CowFileRepository() : base(Path.Combine("Data", "cows.json")) { }

    public async Task<Cow> AddAsync(Cow cow)
    {
        var items = await LoadAsync();
        cow.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(cow);
        await SaveAsync(items);
        return cow;
    }

    public async Task UpdateAsync(Cow cow)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == cow.Id);
        if (idx < 0) throw new NotFoundException("Cow not found.");
        items[idx] = cow;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Cow not found.");
        await SaveAsync(items);
    }

    public async Task<Cow> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Cow not found.");
    }

    public IQueryable<Cow> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
