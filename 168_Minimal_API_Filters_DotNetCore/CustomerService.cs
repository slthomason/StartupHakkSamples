using MinimalAPIWithFilters.Model;

namespace MinimalAPIWithFilters;

public class CustomerService
{
    private List<Customer> _customers = new List<Customer>();

    public Customer GetCustomer(int id) => 
        _customers.FirstOrDefault(c => c.Id == id);

    public void AddCustomer(Customer customer) => 
        _customers.Add(customer);

    public void UpdateCustomer(int id, Customer updatedCustomer)
    {
        var customer = GetCustomer(id);
        if (customer != null)
        {
            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
        }
    }

    public void DeleteCustomer(int id)
    {
        var customer = GetCustomer(id);
        if (customer != null)
        {
            _customers.Remove(customer);
        }
    }
}