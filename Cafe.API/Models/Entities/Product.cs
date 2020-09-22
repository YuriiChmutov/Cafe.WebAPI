using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafe.API.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)] //to display two symbols after the dot
        [Range(0.01, double.MaxValue, ErrorMessage = "Price should be more than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0.01, float.MaxValue)]
        public float Weight { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public int Calories { get; set; }

        public int CurrentCategoryId { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<ClientProduct> ClientProducts { get; set; }
    }
}
