using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IDataRepository<Client> _dataRepository;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IDataRepository<Client> dataRepository,
            ILogger<ClientController> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        // GET: api/clients
        // ?sort=desc||asc
        [HttpGet]
        public IEnumerable<Client> Get(string sort)
        {
            try
            {
                IEnumerable<Client> clients;

                switch (sort)
                {
                    case "asc":
                        clients = _dataRepository.GetAll().OrderBy(x => x.SecondName);
                        break;
                    case "desc":
                        clients = _dataRepository.GetAll().OrderByDescending(x => x.SecondName);
                        break;
                    default:
                        clients = _dataRepository.GetAll();
                        break;
                }

                _logger.LogInformation($"Collection of clients was found");
                return clients;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return null;
            }
        }

        // GET api/clients/5
        [HttpGet("{id}", Name = "GetClient")]
        public Client Get(int id)
        {
            try
            {
                return _dataRepository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return null;
            }
        }

        // POST api/clients
        [HttpPost]
        public void Post([FromBody] Client client)
        {
            _dataRepository.Add(client);
        }

        // PUT api/clients/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Client client)
        {
            var entity = _dataRepository.Get(id);
            _dataRepository.Update(entity, client);
        }

        // DELETE api/clients/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _dataRepository.Get(id);
            _dataRepository.Delete(entity);
        }

        [HttpGet("[action]")]
        //PagingClients?pageNumber=1&&pageSize=5
        public IEnumerable<Client> PagingClients(int? pageNumber, int? pageSize)
        {
            try
            {
                var clients = _dataRepository.GetAll();
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;

                _logger.LogInformation($"Amount of clients is {pageSize} on the page {pageNumber}");

                return clients.Skip((currentPageNumber - 1)
                    * currentPageSize).Take(currentPageSize);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return null;
            }
        }

        [HttpGet]
        [Route("[action]")]
        //SearchClients?clientName=
        public IEnumerable<Client> SearchClients(string clientName)
        {
            try
            {
                var clients = _dataRepository.GetAll()
                    .Where(c => c.SecondName.ToLower()
                    .StartsWith(clientName.ToLower()));

                if (clients == null)
                {
                    _logger.LogInformation($"Clients whose name begin with {clientName} not found");
                    return null;
                }

                _logger.LogInformation($"Clients whose name begin with {clientName} was found");
                return clients;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return null;
            }
        }
    }
}