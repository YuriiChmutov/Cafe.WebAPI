using Cafe.API.Models.Entities;
using Cafe.API.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Cafe.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDataRepository<Product> _dataRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IDataRepository<Product> dataRepository,
            ILogger<ProductController> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_dataRepository.GetAll());
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get(int id)
        {
            try
            {
                var product = _dataRepository.Get(id);

                if (product == null)
                {
                    return NotFound("Product not found");
                }
                return Ok(product);
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Method returned null reference: {ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // POST api/products
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                if (product is null)
                {
                    return BadRequest("Product is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data is incorrect");
                }
                else
                {
                    _dataRepository.Add(product);
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            try
            {
                var entity = _dataRepository.Get(id);
                if (entity == null)
                {
                    return NotFound("Product not found");
                }
                else
                {
                    _dataRepository.Update(entity, product);
                    return Ok("Product was updated succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = _dataRepository.Get(id);
                if (entity == null)
                {
                    return NotFound("Product not found");
                }
                else
                {
                    _dataRepository.Delete(entity);
                    return Ok("Product was deleted succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
    }
}
