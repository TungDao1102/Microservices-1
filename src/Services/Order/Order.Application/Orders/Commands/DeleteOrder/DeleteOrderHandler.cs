using BuildingBlocks.CQRS;
using Order.Application.Data;
using Order.Application.Exceptions;
using Order.Domain.ValueObjects;

namespace Order.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(request.OrderId);
            var order = await dbContext.Orders
                .FindAsync([orderId], cancellationToken: cancellationToken) ?? throw new OrderNotFoundException(request.OrderId);

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
