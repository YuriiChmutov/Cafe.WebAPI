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
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IDataRepository<Category> _dataRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IDataRepository<Category> dataRepository,
            ILogger<CategoryController> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        // GET: api/categories
        [HttpGet]
        public IActionResult Get(string sort)
        {
            try
            {
                IEnumerable<Category> categories;

                switch (sort)
                {
                    case "desc":
                        categories = _dataRepository.GetAll().OrderByDescending(x => x.Name);
                        break;
                    case "asc":
                        categories = _dataRepository.GetAll().OrderBy(x => x.Name);
                        break;
                    default:
                        categories = _dataRepository.GetAll();
                        break;
                }

                _logger.LogInformation("Collection of categories was found succesfully");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Collection of categories wasn't found, {ex.Message}");
                return NotFound();
            }
        }

        // GET api/categories/5
        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult Get(int id)
        {
            try
            {
                var category = _dataRepository.Get(id);

                if (category == null)
                {
                    _logger.LogWarning($"The searching category with id {id} wasn't found");
                    return NotFound();
                }

                _logger.LogInformation($"The category {id}: {category.Name} - was found");
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            try
            {
                if (category is null)
                {
                    _logger.LogWarning("Category is null");
                    return BadRequest("Category is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Validation mistake for category.Id = {category.Id}");
                    return BadRequest();
                }
                else
                {
                    _dataRepository.Add(category);
                    //return CreatedAtRoute("GetCategory", new { Id = category.Id }, null);
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            try
            {
                var entity = _dataRepository.Get(id);
                if (entity == null)
                {
                    _logger.LogWarning($"Category with Id {id} not found");
                    return NotFound();
                }
                else
                {
                    _dataRepository.Update(entity, category);
                    _logger.LogInformation("Category was updated succesfully");
                    return Ok("Data was updated succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = _dataRepository.Get(id);

                if (entity == null)
                {
                    _logger.LogWarning($"Category with Id {id} not found");
                    return NotFound();
                }

                _dataRepository.Delete(entity);
                _logger.LogInformation($"Category with Id {id} was deleted");
                return Ok("Data was deleted succesfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("[action]")]
        //PagingClients?pageNumber=1&&pageSize=5
        public IActionResult PagingCategories(int? pageNumber, int? pageSize)
        {
            try
            {
                var categories = _dataRepository.GetAll();
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;

                _logger.LogInformation($"Amount of categories is {pageSize} on the page {pageNumber}");

                return Ok(categories.Skip((currentPageNumber - 1)
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
        //SearchClients?clientName=
        public IActionResult SearchCategories(string categoryName)
        {
            try
            {
                var categories = _dataRepository.GetAll()
                    .Where(c => c.Name.ToLower()
                    .StartsWith(categoryName.ToLower()));

                if (categories == null)
                {
                    _logger.LogWarning($"The categories which begin with {categoryName} not found");
                    return NotFound();
                }

                _logger.LogInformation("Data found succesfully");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
    }
}
