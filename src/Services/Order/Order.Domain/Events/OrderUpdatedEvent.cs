using Order.Domain.Abstractions;

namespace Order.Domain.Events
{
    public record OrderUpdatedEvent(Models.Order order) : IDomainEvent;
}
