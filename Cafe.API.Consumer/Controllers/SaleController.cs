using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Cafe.API.Models.SortingParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.API.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IDataRepository<ClientProduct> _dataRepository;
        private readonly ILogger<SaleController> _logger;

        public SaleController(IDataRepository<ClientProduct> dataRepository,
            ILogger<SaleController> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        // GET: api/sales
        [HttpGet]
        public IActionResult Get(DirectionSort? sort)
        {
            try
            {
                IEnumerable<ClientProduct> sales;

                switch (sort)
                {
                    case DirectionSort.Ascending:
                        sales = _dataRepository.GetAll().OrderBy(x => x.Date);
                        break;
                    case DirectionSort.Descending:
                        sales = _dataRepository.GetAll().OrderByDescending(x => x.Date);
                        break;
                    default:
                        sales = _dataRepository.GetAll();
                        break;
                }

                _logger.LogInformation($"Sales was found succesfully");
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // GET api/sales/5
        // id - it is a client id, action returns all sales of cusomer with this id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var sales = _dataRepository.GetSales(id);

                if(sales == null)
                {
                    _logger.LogWarning($"Client with Id {id} not found");
                    return NotFound($"Can't find client with id {id}");
                }
                else
                {
                    _logger.LogInformation($"Sales of client with Id {id} were found");
                    return Ok(sales);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // POST api/sales
        [HttpPost]
        public IActionResult Post([FromBody] ClientProduct sale)
        {
            try
            {
                if (sale is null)
                {
                    _logger.LogWarning($"Sale is null reference");
                    return BadRequest("Client or product is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Validation error");
                    return BadRequest("Data is incorrect");
                }
                else
                {
                    _dataRepository.Add(sale);
                    _logger.LogInformation($"New sale was created succesfully");
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        //// PUT api/<SaleController>/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] ClientProduct sale)
        //{
        //}

        //// DELETE api/<SaleController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        /// <summary>
        /// - provide the page Number
        /// - provide the page Size
        /// - get all the data from the database
        /// - apply the simple Skip and Take algorithm to get the simple specific result 
        /// - quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize)
        /// </summary>
        [HttpGet("[action]")]
        //PagingSales?pageNumber=1&&pageSize=3
        public IActionResult PagingSales(int? pageNumber, int? pageSize)
        {
            try
            {
                var sales = _dataRepository.GetAll();
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;

                _logger.LogInformation($"Amount of sales on the page {pageNumber}");
                return Ok(sales.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpGet]
        [Route("[action]")]
        // SearchSales?clientId=1&&clientName=2
        public IActionResult SearchSales(string clientName) //int? clientId,
        {
            try
            {
                var sales = _dataRepository.GetAll()
                    .Where(s => s.Client.SecondName.ToLower()
                    .StartsWith(clientName.ToLower()));
                        // || s.ClientId == clientId);

                if (sales == null)
                {
                    _logger.LogWarning($"{clientName}'s sale not found");
                    return NotFound();
                }

                _logger.LogInformation($"Sales for client {clientName} was found");
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}
