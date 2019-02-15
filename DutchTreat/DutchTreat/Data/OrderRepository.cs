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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly ProductContext _ctx;
        private readonly ILogger<IOrderRepository> _logger;

        public OrderRepository(ProductContext ctx, ILogger<IOrderRepository> logger)
        : base(ctx)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _ctx.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product).ToListAsync();     
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _ctx.Orders.Where(o => o.Id.Equals(id))
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync();     
        }        
    }
}