using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGreat.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new List<Product>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        [NotMapped]
        public IList<Product> Products { get; set; }

        public decimal TotalPrice { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
