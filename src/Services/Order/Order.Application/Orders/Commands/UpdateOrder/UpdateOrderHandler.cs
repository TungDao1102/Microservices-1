using BuildingBlocks.CQRS;
using Order.Application.Data;
using Order.Application.Dtos;
using Order.Application.Exceptions;
using Order.Domain.ValueObjects;

namespace Order.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(request.Order.Id);
            var order = await dbContext.Orders
                .FindAsync([orderId], cancellationToken: cancellationToken) ?? throw new OrderNotFoundException(request.Order.Id);

            UpdateOrderWithNewValues(order, request.Order);

            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        public static void UpdateOrderWithNewValues(Domain.Models.Order order, OrderDto orderDto)
        {
            var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
            var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
            var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

            order.Update(
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: updatedShippingAddress,
                billingAddress: updatedBillingAddress,
                payment: updatedPayment,
                status: orderDto.Status);
        }
    }
}
