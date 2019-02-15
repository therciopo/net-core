using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly ProductContext _ctx;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductContext ctx, ILogger<ProductRepository> logger)
        : base(ctx)
        {
            _ctx = ctx;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            _logger.LogInformation("GetAll was called");

            var products =  await FindAllAsync();
            return products.OrderBy(p => p.Title);                
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            return await _ctx.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
