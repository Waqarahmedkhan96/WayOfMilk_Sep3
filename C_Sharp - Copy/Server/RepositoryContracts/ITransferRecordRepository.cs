using Entities;

namespace RepositoryContracts;

public interface ITransferRecordRepository
{
    Task<TransferRecord> AddAsync(TransferRecord record);
    Task UpdateAsync(TransferRecord record);
    Task DeleteAsync(int id);
    Task<TransferRecord> GetSingleAsync(int id);
    IQueryable<TransferRecord> GetManyAsync();
}
