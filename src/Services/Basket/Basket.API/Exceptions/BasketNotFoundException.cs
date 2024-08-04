using BuildingBlocks.Exceptions;

namespace Basket.API.Exceptions
{
    public class BasketNotFoundException(string message) : NotFoundException(message)
    {
    }
}
