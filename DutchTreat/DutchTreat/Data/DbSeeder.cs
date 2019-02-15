using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DutchTreat.Data
{
    public class DbSeeder
    {
        private readonly ProductContext _ctx;
        private readonly IHostingEnvironment _hosting;

        public DbSeeder(ProductContext ctx, IHostingEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();
            if(!_ctx.Products.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var orders = new List<Order>()
                {
                    new Order()
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = "12345",
                        Items = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Id=1,
                                Product = products.First(),
                                Quantity = 5,
                                UnitPrice = products.First().Price
                            },
                            new OrderItem()
                            {
                                Id=2,
                                Product = products.ElementAt(2),
                                Quantity = 2,
                                UnitPrice = products.ElementAt(2).Price

                            }
                        }
                    },
                    new Order()
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = "54321",
                        Items = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Id=3,
                                Product = products.Last(),
                                Quantity = 1,
                                UnitPrice = products.Last().Price
                            }
                        }
                    }
                };

                _ctx.Orders.AddRange(orders);

                _ctx.SaveChanges();
            }
        }
    }
}
