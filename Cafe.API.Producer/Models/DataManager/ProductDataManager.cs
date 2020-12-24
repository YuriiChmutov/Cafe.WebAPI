using Cafe.API.Data;
using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Models.DataManager
{
    public class ProductDataManager: IDataRepository<Product>
    {
        readonly CafeDbContext _context;
        public ProductDataManager(CafeDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

        public Product Get(int id)
        {
            var product = _context.Products.Find(id);
            return product;
        }

        public Product GetLast()
        {
            return _context.Products.Last();
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product productToUpdate, Product product)
        {
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.Weight = product.Weight;
            productToUpdate.Calories = product.Calories;
            productToUpdate.CurrentCategoryId = product.CurrentCategoryId;

            _context.SaveChanges();
        }

        public void Delete(Product product)
        {
            _context.Remove(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetSales(int clientId)
        {
            return _context.Products;
        }

        public int Count()
        {
            return _context.Products.Count();
        }
    }
}
