using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class MilkFileRepository : FileRepository<Milk>, IMilkRepository
{
    public MilkFileRepository() : base(Path.Combine("Data", "milks.json")) { }

    public async Task<Milk> AddAsync(Milk milk)
    {
        // basic checks
        if (milk.VolumeL < 0) throw new ValidationException("Volume must be >= 0.");

        // rule: if approved, test must be PASS
        if (milk.ApprovedForStorage && milk.MilkTestResult != MilkTestResult.Pass)
            throw new ValidationException("Milk can be approved only when test result is PASS.");

        // rule: cannot assign container before approval
        if (!milk.ApprovedForStorage && milk.ContainerId is not null)
            throw new ValidationException("Cannot assign a container before milk is approved.");

        var items = await LoadAsync();
        milk.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(milk);
        await SaveAsync(items);
        return milk;
    }

    public async Task UpdateAsync(Milk milk)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == milk.Id);
        if (idx < 0) throw new NotFoundException("Milk not found.");

        if (milk.VolumeL < 0) throw new ValidationException("Volume must be >= 0.");

        // rule: if approved, test must be PASS
        if (milk.ApprovedForStorage && milk.MilkTestResult != MilkTestResult.Pass)
            throw new ValidationException("Milk can be approved only when test result is PASS.");

        // rule: cannot assign container before approval
        if (!milk.ApprovedForStorage && milk.ContainerId is not null)
            throw new ValidationException("Cannot assign a container before milk is approved.");

        items[idx] = milk;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Milk not found.");
        await SaveAsync(items);
    }

    public async Task<Milk> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Milk not found.");
    }

    public IQueryable<Milk> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
