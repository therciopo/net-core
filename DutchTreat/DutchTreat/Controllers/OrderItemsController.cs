using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IOrderRepository _orderRepository;

        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;


        public OrderItemsController(IProductRepository repository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ILogger<OrderItemsController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int orderid)
        {
            var order = await _orderRepository.GetOrderById(orderid);

            var vm = _mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items);

            if(vm != null)
            {
                return Ok(vm);
            }
            return NotFound();

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int orderid, int id)
        {
            var order = await _orderRepository.GetOrderById(orderid);
            if (order != null)
            {
                var item = await _orderItemRepository.GetOrderItemById(id);
                var vm = _mapper.Map<OrderItem, OrderItemViewModel>(item);
                return Ok(vm);
            }
            return NotFound();
        }
    }
}
