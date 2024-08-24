using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Order.Application.Data;
using Order.Application.Extensions;

namespace Order.Application.Orders.Queries.GetOrderByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
               .AsNoTracking()
               .Where(o => o.OrderName.Value.Contains(request.Name))
               .OrderBy(o => o.OrderName.Value)
               .ToListAsync(cancellationToken);

            return new GetOrdersByNameResult(orders.ToOrderDtoList());
        }
    }
}
