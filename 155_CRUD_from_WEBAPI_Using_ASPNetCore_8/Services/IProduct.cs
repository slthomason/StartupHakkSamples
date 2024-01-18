using WebApplication24.Models;

namespace WebApplication24.Services
{
    public interface IProduct
    {
        List<Products> GetAllProducts(bool? isActive);

        Products? GetProductsByID(int id);

        Products AddProducts(AddUpdateProducts obj);

        Products? UpdateProducts(int id, AddUpdateProducts obj);

        bool DeleteProductsByID(int id);
    }
}
