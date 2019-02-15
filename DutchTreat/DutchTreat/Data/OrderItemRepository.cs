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
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        private readonly ProductContext _ctx;
        private readonly ILogger<IOrderItemRepository> _logger;

        public OrderItemRepository(ProductContext ctx, ILogger<IOrderItemRepository> logger)
        : base(ctx)
        {
            _ctx = ctx;
            _logger = logger;
        }
        public async Task<OrderItem> GetOrderItemById(int id)
        {
            var item = await FindByConditionAync(o => o.Id.Equals(id));

            return item.FirstOrDefault();
        }
    }
}
