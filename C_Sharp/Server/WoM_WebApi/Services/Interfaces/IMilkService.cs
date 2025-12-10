using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: milk records
public interface IMilkService
{
    Task<MilkDto> CreateAsync(CreateMilkDto dto);                 // create record
    Task<MilkDto> GetByIdAsync(long id);                          // get one
    Task<MilkListDto> GetAllAsync();                              // list all
    Task<MilkListDto> GetByContainerAsync(long containerId);      // filter by container
    Task<MilkDto> UpdateAsync(UpdateMilkDto dto);                 // update record
    Task ApproveStorageAsync(ApproveMilkStorageDto dto);          // approve/deny storage
    Task DeleteAsync(long id);                                    // delete record
}
