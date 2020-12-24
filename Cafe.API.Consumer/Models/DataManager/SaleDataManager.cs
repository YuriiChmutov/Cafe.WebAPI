using Cafe.API.Data;
using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Models.DataManager
{
    public class SaleDataManager: IDataRepository<ClientProduct>
    {
        CafeDbContext _context;
        public SaleDataManager(CafeDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<ClientProduct> GetAll()
        {
            return _context.Sales;
        }

        public ClientProduct Get(int clientId)
        {
            var sale = _context.Sales.Find(clientId);
            return sale;
        }

        public IEnumerable<ClientProduct> GetSales(int clientId)
        {
            var sales = _context.Sales.Where(x => x.ClientId == clientId);
            return sales;
        }

        public void Add(ClientProduct sale)
        {
            sale.Date = DateTime.Now;
            _context.Sales.Add(sale);
            _context.SaveChanges();
        }

        public void Update(ClientProduct saleToUpdate, ClientProduct sale)
        {
            var lastClientSale = _context.Sales.Where(x => x.ClientId == sale.ClientId).Last();
            lastClientSale.ProductId = sale.ProductId;

            _context.SaveChanges();
        }

        public void Delete(ClientProduct sale)
        {
            _context.Remove(sale);
            _context.SaveChanges();
        }
    }
}
