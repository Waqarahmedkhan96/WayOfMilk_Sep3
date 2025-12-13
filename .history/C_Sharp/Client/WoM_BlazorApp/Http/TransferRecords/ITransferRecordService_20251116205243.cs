using ApiContracts.TransferRecords;

namespace WoM_BlazorApp.Http;

public interface ITransferRecordService
{
    Task<ICollection<TransferRecordDto>> GetAllAsync();
    Task<TransferRecordDto> GetByIdAsync(int id);
    Task<TransferRecordDto> CreateAsync(CreateTransferRecordDto dto);
    Task UpdateAsync(int id, UpdateTransferRecordDto dto);
    Task DeleteAsync(int id);
}
