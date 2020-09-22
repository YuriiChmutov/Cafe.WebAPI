using Cafe.API.Data;
using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using System.Collections.Generic;

namespace Cafe.API.Models.DataManager
{
    public class ClientDataManager: IDataRepository<Client>
    {
        CafeDbContext _context;

        public ClientDataManager(CafeDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Client> GetAll()
        {
            return _context.Clients;
        }

        public Client Get(int id)
        {
            var client = _context.Clients.Find(id);
            return client;
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void Update(Client clientToUpdate, Client client)
        {
            clientToUpdate.Name = client.Name;
            clientToUpdate.SecondName = client.SecondName;
            clientToUpdate.Age = client.Age;

            _context.SaveChanges();
        }

        public void Delete(Client client)
        {
            _context.Remove(client);
            _context.SaveChanges();
        }

        public IEnumerable<Client> GetSales(int clientId)
        {
            return _context.Clients;
        }
    }
}
