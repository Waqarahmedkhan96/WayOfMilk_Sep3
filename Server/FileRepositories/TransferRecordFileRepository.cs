using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class TransferRecordFileRepository : FileRepository<TransferRecord>, ITransferRecordRepository
{
    public TransferRecordFileRepository() : base(Path.Combine("Data", "transfers.json")) { }

    public async Task<TransferRecord> AddAsync(TransferRecord tr)
    {
        var items = await LoadAsync();
        tr.Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1;
        items.Add(tr);
        await SaveAsync(items);
        return tr;
    }

    public async Task UpdateAsync(TransferRecord tr)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(x => x.Id == tr.Id);
        if (idx < 0) throw new NotFoundException("Transfer record not found.");
        items[idx] = tr;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(x => x.Id == id);
        if (removed == 0) throw new NotFoundException("Transfer record not found.");
        await SaveAsync(items);
    }

    public async Task<TransferRecord> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(x => x.Id == id)
               ?? throw new NotFoundException("Transfer record not found.");
    }

    public IQueryable<TransferRecord> GetManyAsync()
        => LoadAsync().Result.AsQueryable();
}
