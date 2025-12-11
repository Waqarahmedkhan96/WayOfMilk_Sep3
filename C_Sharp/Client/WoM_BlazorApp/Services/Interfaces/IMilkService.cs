using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface IMilkService
{
    Task<MilkListDto> GetAllAsync();                 // get all
    Task<MilkDto> GetByIdAsync(long id);             // get one
    Task<MilkListDto> GetByContainerAsync(long id);  // by container
    Task<MilkDto> CreateAsync(CreateMilkDto dto);    // create
    Task<MilkDto> UpdateAsync(long id, UpdateMilkDto dto); // update
    Task ApproveAsync(long id, bool approved);       // approve
    Task DeleteAsync(long id);                       // delete
}
