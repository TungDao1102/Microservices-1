using BuildingBlocks.CQRS;
using Order.Application.Dtos;

namespace Order.Application.Orders.Queries.GetOrderByName
{
    public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;

    public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
}
