using Cafe.API.Data;
using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Models.DataManager
{
    public class CategoryDataManager: IDataRepository<Category>
    {
        readonly CafeDbContext _context;

        public CategoryDataManager(CafeDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category Get(int id)
        {
            var category = _context.Categories.Find(id);
            return category;
        }

        public Category GetLast()
        {
            var category = _context.Categories.Last();
            return category;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category categoryToUpdate, Category category)
        {
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;

            _context.SaveChanges();
        }

        public void Delete(Category category)
        {
            _context.Remove(category);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetSales(int clientId)
        {
            return _context.Categories;
        }

        public int Count()
        {
            return _context.Categories.Count();
        }
    }
}
