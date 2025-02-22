using CustomerManagerWeb.Models;

namespace CustomerManagerWeb.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<MessageResponse<List<Customer>>> GetAllAsync();
        MessageResponse<Customer> GetById(int id);
        MessageResponse<Customer> Create(Customer customer);
        MessageResponse<Customer> Update(Customer customer);
        MessageResponse<object> Delete(int id);
        MessageResponse<List<Customer>> GetTopCustomers();

        MessageResponse<Address> GetAddressById(int id);
        MessageResponse<Address> CreateAddress(Address address);
        MessageResponse<Address> UpdateAddress(Address address);
        MessageResponse<object> DeleteAddress(int id);
    }
}
