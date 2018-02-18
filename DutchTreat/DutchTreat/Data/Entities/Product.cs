using System;

namespace DutchTreat.Data.Entities
{
    public class Product
  {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDating { get; set; }
        public decimal StarRating { get; set; }
        public string ImageUrl { get; set; }
    }
}
