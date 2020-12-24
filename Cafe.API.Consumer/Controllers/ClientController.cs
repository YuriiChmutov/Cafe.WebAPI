using Cafe.API.Models.Entities;
using Cafe.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;

        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetClient(int Id)
        {
            var client = new HttpClient();
            string resultJson = await client
                .GetStringAsync($"https://localhost:3001/api/clients/GetClientById?Id={Id}");
            Client person = JsonConvert
                .DeserializeObject<Client>(resultJson);
            return Ok(person);
        }

        // GET: api/clients
        // ?sort=desc||asc
        [HttpGet]
        public async Task<IActionResult> Get(string sort)
        {
            try
            {
                var client = new HttpClient();
                string resultJson = await client
                    .GetStringAsync($"https://localhost:3001/api/clients?sort={sort}");
                IEnumerable<Client> persons = JsonConvert.
                    DeserializeObject<IEnumerable<Client>>(resultJson);
                
                _logger.LogInformation($"Collection of clients was found");
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // GET api/clients/5
        [HttpGet("{id}", Name = "GetClient")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var client = new HttpClient();
                string resultJson = await client
                    .GetStringAsync($"https://localhost:3001/api/clients/{id}");
                Client person = JsonConvert
                    .DeserializeObject<Client>(resultJson);

                if (person == null)
                {
                    _logger.LogWarning($"Client with Id {id} not found");
                    return NotFound("Sorry, I can't find this client");
                }

                var isPersonHungry = person.IsHungry? "Hungry" : "Not Hyngry";
                _logger.LogInformation($"{person.Name} {person.SecondName} is {isPersonHungry}.");
                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // POST api/clients
        //[HttpPost("[action]")]
        [HttpPost]
        public async Task<IActionResult> Post()//[FromBody] Client client
        {
            try
            {
                var rand = new Random();
                var client = new Client()
                {
                    IsHungry = true,
                    Name = ClientParams.Names[GetRandom.returnRandom(ClientParams.Names.Count)],
                    SecondName = ClientParams.SecondNames[GetRandom.returnRandom(ClientParams.SecondNames.Count)],
                    Age = rand.Next(15, 80)
                };

                var httpClient = new HttpClient();
                var jsonInString = JsonConvert.SerializeObject(client);
                var httpResponse = await httpClient
                    .PostAsync("https://localhost:3001/api/clients",
                        new StringContent(jsonInString, Encoding.UTF8, "application/json"));

                httpResponse.EnsureSuccessStatusCode();
                return StatusCode(StatusCodes.Status201Created, $"{client.Name} {client.SecondName} has came.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened on the server while handing your request");
            }
        }

        // PUT api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Client client)
        {
            try
            {
                var httpClient = new HttpClient();
                var jsonInString = JsonConvert.SerializeObject(client);
                await httpClient.PutAsync($"https://localhost:3001/api/clients/{id}",
                                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

                return StatusCode(StatusCodes.Status200OK, $"Client was updated!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened on the server while handing your request");
            }
        }

        // DELETE api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = new HttpClient();
                await client.DeleteAsync($"https://localhost:3001/api/clients/{id}");
                return Ok("Was deleted succesfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened on the server while handing your request");
            }
        }

        [HttpGet("[action]")]
        //PagingClients?pageNumber=1&&pageSize=5
        public async Task<IActionResult> PagingClients(int? pageNumber, int? pageSize)
        {
            try
            {
                var client = new HttpClient();
                string resultJson = await client
                    .GetStringAsync($"https://localhost:3001/api/clients/PagingClients?pageNumber={pageNumber}&&pageSize={pageSize}");
                IEnumerable<Client> persons = JsonConvert.
                    DeserializeObject<IEnumerable<Client>>(resultJson);

                _logger.LogInformation($"Amount of clients is {pageSize} on the page {pageNumber}");

                return Ok(persons);
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
        public async Task<IActionResult> SearchClients(string clientName)
        {
            try
            {
                var client = new HttpClient();
                string resultJson = await client
                    .GetStringAsync($"https://localhost:3001/api/clients/SearchClients?clientName={clientName}");
                IEnumerable<Client> persons = JsonConvert.
                    DeserializeObject<IEnumerable<Client>>(resultJson);
                                
                if (persons == null)
                {
                    _logger.LogInformation($"Clients whose name begin with {clientName} not found");
                    return NotFound();
                }

                _logger.LogInformation($"Clients whose name begin with {clientName} was found");
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
    }
}