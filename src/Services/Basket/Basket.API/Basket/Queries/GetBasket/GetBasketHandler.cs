using Basket.API.Models;
using BuildingBlocks.CQRS;

namespace Basket.API.Basket.Queries.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart Cart);

    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
