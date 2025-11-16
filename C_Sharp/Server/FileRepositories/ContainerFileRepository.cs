using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class ContainerFileRepository : FileRepository<Container>, IContainerRepository
{
    public ContainerFileRepository() : base(Path.Combine("Data", "containers.json")) { }

    public async Task<Container> AddAsync(Container container)
    {
        var items = await LoadAsync();
        container.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(container);
        await SaveAsync(items);
        return container;
    }

    public async Task UpdateAsync(Container container)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == container.Id);
        if (idx < 0) throw new NotFoundException("Container not found.");
        items[idx] = container;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Container not found.");
        await SaveAsync(items);
    }

    public async Task<Container> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Container not found.");
    }

    public IQueryable<Container> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
