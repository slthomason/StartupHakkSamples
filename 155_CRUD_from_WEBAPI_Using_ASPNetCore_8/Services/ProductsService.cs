using WebApplication24.Models;

namespace WebApplication24.Services
{
    public class ProductsService:IProduct
    {
        private readonly List<Products> _productList;

        public ProductsService()
        {
            _productList = new List<Products>()
            {
                new Products()
                {
                    Id=1,
                    Name="iPhone",
                    Description="Apple",
                    isActive=true,
                }
            };
        }
        public List<Products> GetAllProducts(bool? isActive)
        {
            return isActive == null ? _productList : _productList.Where(item => item.isActive == isActive).ToList();

        }
        public Products? GetProductsByID(int id)
        {
            return _productList.FirstOrDefault(item => item.Id == id);
        }

        public Products? AddProducts(AddUpdateProducts obj)
        {
            var addProduct = new Products()
            {
                Id=_productList.Max(item => item.Id) +1,
                Name= obj.Name,
                Description= obj.Description,
                isActive = obj.isActive
            };
            _productList.Add(addProduct);
            return addProduct;
        }

        public Products? UpdateProducts(int id, AddUpdateProducts obj)
        {
            var product = _productList.FindIndex(item => item.Id==id);
            if (product > 0)
            {
                var item = _productList[product];

                item.Name = obj.Name;
                item.Description = obj.Description;
                item.isActive=obj.isActive;

                _productList[product] = item;

                return item;
               
            }
            else
            { 
                return null;
            }
        }

        public bool DeleteProductsByID(int id)
        {
            var product = _productList.FindIndex(item => item.Id == id);
            if (product>=0)
            {
                _productList.RemoveAt(product);
            }
            return product >=0;
        }

    }
}
