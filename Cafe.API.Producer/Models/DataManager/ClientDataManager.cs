using Cafe.API.Data;
using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Models.DataManager
{
    public class ClientDataManager: IDataRepository<Client>
    {
        readonly CafeDbContext _context;

        public ClientDataManager(CafeDbContext context)
        {
            this._context = context;
        }

        public Client GetLast()
        {
            return _context.Clients
                .Where(x => x.IsHungry == true)
                .OrderBy(x => x.TimeOfComing)
                .First();
            //return _context.Clients.Last();
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
            client.TimeOfComing = DateTime.Now;
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void Update(Client clientToUpdate, Client client)
        {
            clientToUpdate.Name = client.Name;
            clientToUpdate.SecondName = client.SecondName;
            clientToUpdate.Age = client.Age;
            clientToUpdate.IsHungry = client.IsHungry;
            clientToUpdate.TimeOfComing = client.TimeOfComing;

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

        public int Count()
        {
            return _context.Clients.Count();
        }
    }
}
