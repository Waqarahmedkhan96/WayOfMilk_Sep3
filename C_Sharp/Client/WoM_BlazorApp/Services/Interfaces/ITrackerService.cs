using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface ITrackerService
{
    //  Get the starting point
    Task<SaleDto> GetSaleByIdAsync(long id);
    Task<CowDto> GetCowByIdAsync(long id);
    Task<IEnumerable<SaleDto>> GetAllTrackedSalesAsync();

    //  The "Mock" Tracking Methods (Loads ALL entities)
    Task<IEnumerable<CowDto>> GetInvolvedCowsAsync();
    Task<IEnumerable<CustomerDto>> GetInvolvedCustomersAsync();
    Task<IEnumerable<ContainerDto>> GetInvolvedContainersAsync();

    //  The Bulk Action
    Task MarkCowsAsSuspectAsync(IEnumerable<long> cowIds);
}