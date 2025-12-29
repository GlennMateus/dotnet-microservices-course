using Discount.Grpc;
using JasperFx.Events.Daemon;

namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart)
            .NotNull()
            .WithMessage($"{nameof(StoreBasketCommand.Cart)} is required");

        RuleFor(x => x.Cart.UserName)
            .NotEmpty()
            .WithMessage($"{nameof(StoreBasketCommand.Cart.UserName)} is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository
    , DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await ApplyDiscount(command.Cart, cancellationToken);

        await repository.StoreBasket(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task ApplyDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var cupon = await discountProto
                .GetDiscountAsync(
                    new GetDiscountRequest
                    {
                        ProductName = item.ProductName,
                    }
                    , cancellationToken: cancellationToken);
            item.Price -= cupon.Amount * item.Quantity;
        }
    }
}
