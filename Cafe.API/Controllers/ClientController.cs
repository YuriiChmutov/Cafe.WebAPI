using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Get(string sort)
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


                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // GET api/clients/5
        [HttpGet("{id}", Name = "GetClient")]
        public IActionResult Get(int id)
        {
            try
            {
                var client = _dataRepository.Get(id);

                if (client == null)
                {
                    return NotFound("Sorry, I can't find this client");
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // POST api/clients
        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                if (client is null)
                {
                    return BadRequest("Client is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data is incorrect");
                }
                _dataRepository.Add(client);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // PUT api/clients/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            try
            {
                var entity = _dataRepository.Get(id);

                if (entity == null)
                {
                    return NotFound("We can't find this client");
                }
                else
                {
                    _dataRepository.Update(entity, client);
                    return Ok("Data was updated succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }   

        // DELETE api/clients/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = _dataRepository.Get(id);

                if (entity == null)
                {
                    _logger.LogWarning($"Client with id {id} not found");
                    return NotFound("Sorry we can't find this clint");
                }

                _dataRepository.Delete(entity);
                _logger.LogInformation($"Client whith id {id} was deleted");
                return Ok("Client was delted succesfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("[action]")]
        //PagingClients?pageNumber=1&&pageSize=5
        public IActionResult PagingClients(int? pageNumber, int? pageSize)
        {
            try
            {
                var clients = _dataRepository.GetAll();
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;

                _logger.LogInformation($"Amount of clients is {pageSize} on the page {pageNumber}");

                return Ok(clients.Skip((currentPageNumber - 1)
                    * currentPageSize).Take(currentPageSize));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet]
        [Route("[action]")]
        //SearchClients?clientName=
        public IActionResult SearchClients(string clientName)
        {
            try
            {
                var clients = _dataRepository.GetAll()
                    .Where(c => c.SecondName.ToLower()
                    .StartsWith(clientName.ToLower()));

                if (clients == null)
                {
                    _logger.LogInformation($"Clients whose name begin with {clientName} not found");
                    return NotFound();
                }

                _logger.LogInformation($"Clients whose name begin with {clientName} was found");
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
    }
}
