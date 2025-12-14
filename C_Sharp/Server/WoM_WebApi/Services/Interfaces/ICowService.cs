
using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: cow CRUD + queries
public interface ICowService
{
    Task<CowDto> CreateAsync(CowCreationDto dto);
    Task<CowDto> GetByIdAsync(long id);
    Task<IEnumerable<CowDto>> GetAllAsync();
    Task<CowDto> GetByRegNoAsync(string regNo);

    // Updates
    Task<CowDto> UpdateCowAsync(CowDto dto, long requesterUserId);
    IAsyncEnumerable<CowDto> UpdateBatchAsync(IEnumerable<CowDto> dtos, long requesterUserId);
    IAsyncEnumerable<CowDto> UpdateCowsHealthAsync(IEnumerable<long> cowIds, bool healthUpdate, long requesterUserId);

    // Deletes
    Task DeleteAsync(long id);
    Task DeleteBatchAsync(long[] ids);

    //other methods
    Task<IEnumerable<CowDto>> GetCowsByDepartmentAsync(long departmentId);
    Task<IEnumerable<MilkDto>> GetMilkByCowIdAsync(long cowId);
}
