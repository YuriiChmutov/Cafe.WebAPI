using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafe.API.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You should input name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should input a description")]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
