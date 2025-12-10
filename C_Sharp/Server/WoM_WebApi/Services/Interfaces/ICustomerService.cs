using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: customers
public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);         // create customer
    Task<CustomerDto> GetByIdAsync(long id);                      // get one
    Task<CustomerListDto> GetAllAsync();                          // list all
    Task<CustomerDto?> GetByCvrAsync(string cvr);                 // query by CVR
    Task<CustomerListDto> GetByNameAsync(string namePart);        // query by name
    Task DeleteAsync(long id);                                    // delete
}
