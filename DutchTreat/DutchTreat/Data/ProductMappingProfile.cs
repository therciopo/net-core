using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Data
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(vm => vm.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()                
                .ReverseMap();
        }
    }
}
