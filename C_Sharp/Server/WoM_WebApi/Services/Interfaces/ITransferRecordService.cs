using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: transfer records
public interface ITransferRecordService
{
    Task<TransferRecordDto> CreateAsync(CreateTransferRecordDto dto);      // create transfer
    Task<TransferRecordDto> GetByIdAsync(long id);                         // get one
    Task<TransferRecordListDto> GetAllAsync();                             // list all
    Task<TransferRecordListDto> GetForCowAsync(long cowId);                // list by cow
    Task<TransferRecordDto> UpdateAsync(UpdateTransferRecordDto dto);      // update record
    Task ApproveAsync(ApproveTransferDto dto);                             // vet approves
    Task DeleteAsync(long id);                                             // delete
}