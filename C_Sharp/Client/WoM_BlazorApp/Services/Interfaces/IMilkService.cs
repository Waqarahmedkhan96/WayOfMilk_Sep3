using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface IMilkService
{
    Task<MilkListDto> GetAllAsync();
    Task<MilkDto> GetByIdAsync(long id);
    Task<MilkListDto> GetByContainerAsync(long containerId);

    Task<MilkDto> CreateAsync(CreateMilkDto dto);
    Task<MilkDto> UpdateAsync(long id, UpdateMilkDto dto);

    // FIXED: must match controller + impl
    Task ApproveAsync(long id, ApproveMilkStorageDto dto);

    Task DeleteAsync(long id);
}
