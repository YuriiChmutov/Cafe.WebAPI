using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafe.API.Models.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string SecondName { get; set; }

        [Required]
        [Range(13, 99)]
        [Display(Name = "Сколько вам лет?")]
        public int Age { get; set; }
        public DateTime TimeOfComing { get; set; }
        public bool IsHungry { get; set; }
        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
    }
}
