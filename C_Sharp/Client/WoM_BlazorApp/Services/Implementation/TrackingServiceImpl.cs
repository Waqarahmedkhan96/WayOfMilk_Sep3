using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class TrackingServiceImpl(
    ISaleService saleService,
    ICowService cowService,
    ICustomerService customerService,
    IContainerService containerService
) : ITrackerService
{
    // Get the specific Sale to start the trace
    public async Task<SaleDto> GetSaleByIdAsync(long id)
    {
        return await saleService.GetTrackedByIdAsync(id);
    }
    public async Task<CowDto> GetCowByIdAsync(long id)
    {
        return await cowService.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SaleDto>> GetAllTrackedSalesAsync()
    {
        return await saleService.GetAllTrackedAsync();
    }

    // MOCK: Return ALL cows as if they were involved
    public async Task<IEnumerable<CowDto>> GetInvolvedCowsAsync()
    {
        return await cowService.GetAllAsync();
    }

    // MOCK: Return ALL customers (or filter by ID if we wanted to be smarter later)
    public async Task<IEnumerable<CustomerDto>> GetInvolvedCustomersAsync()
    {
        return await customerService.GetAllTrackedAsync();
    }

    // MOCK: Return ALL containers
    public async Task<IEnumerable<ContainerDto>> GetInvolvedContainersAsync()
    {
        return await containerService.GetAllTrackedAsync();
    }

    // Action: Mark a list of cows as "Sick" (False)
    public async Task MarkCowsAsSuspectAsync(IEnumerable<long> cowIds)
    {
        // "Suspect" implies not healthy, so we set status to false.
        // This uses the Batch Update method from ICowService.
        await cowService.UpdateHealthAsync(cowIds, false);
    }
}