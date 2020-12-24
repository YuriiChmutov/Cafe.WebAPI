using System;

namespace Cafe.API.Models.Entities
{
    public class ClientProduct
    {
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Client Client { get; set; }
        public DateTime Date { get; set; }
    }
}
