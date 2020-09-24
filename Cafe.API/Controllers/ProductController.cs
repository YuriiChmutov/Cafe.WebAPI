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
        public IActionResult Get(string sort)
        {
            try
            {
                IEnumerable<Product> products;

                switch (sort)
                {
                    case "desc":
                        products = _dataRepository.GetAll().OrderByDescending(x => x.Price);
                        break;
                    case "asc":
                        products = _dataRepository.GetAll().OrderBy(x => x.Price);
                        break;
                    default:
                        products = _dataRepository.GetAll();
                        break;
                }

                _logger.LogInformation("Collection of products was found");
                return Ok(products);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
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

                _logger.LogInformation($"Product {product.Name} was found succesfully");
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
                    _logger.LogWarning("Product is null reference");
                    return BadRequest("Product is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validation error");
                    return BadRequest("Data is incorrect");
                }
                else
                {
                    _dataRepository.Add(product);
                    _logger.LogInformation($"New product {product.Name} wass created");
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
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
                    _logger.LogWarning($"Product with Id {id} not found");
                    return NotFound("Product not found");
                }
                else
                {
                    _dataRepository.Update(entity, product);
                    _logger.LogInformation($"Product with Id {id} was updated");
                    return Ok("Product was updated succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
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
                    _logger.LogWarning($"Produc with Id {id} not found");
                    return NotFound("Product not found");
                }
                else
                {
                    _dataRepository.Delete(entity);
                    _logger.LogInformation($"Product with Id {id} was deleted");
                    return Ok("Product was deleted succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("[action]")]
        //PagingProducts?pageNumber=1&&pageSize=5
        public IActionResult PagingProducts(int? pageNumber, int? pageSize)
        {
            try
            {
                var products = _dataRepository.GetAll();
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;

                _logger.LogInformation($"Amount of products is {pageSize} on the page {pageNumber}");

                return Ok(products.Skip((currentPageNumber - 1)
                        * currentPageSize).Take(currentPageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet]
        [Route("[action]")]
        //SearchProductsByCategory?categoryName=
        public IActionResult SearchProductsByCategory(string categoryName)
        {
            try
            {
                var products = _dataRepository.GetAll()
                    .Where(c => c.Category.Name.ToLower()
                    .StartsWith(categoryName.ToLower()));

                if (products == null)
                {
                    _logger.LogWarning($"The products which exsist at category {categoryName} not found");
                    return NotFound();
                }

                _logger.LogInformation("Data found succesfully");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet]
        [Route("[action]")]
        //SearchProducts?productName=
        public IActionResult SearchProducts(string productName)
        {
            try
            {
                var products = _dataRepository.GetAll()
                    .Where(c => c.Name.ToLower()
                    .StartsWith(productName.ToLower()));

                if (products == null)
                {
                    _logger.LogWarning($"The products which begin with {productName} not found");
                    return NotFound();
                }

                _logger.LogInformation("Data found succesfully");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
    }
}
