

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreatOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreatOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage($"{nameof(OrderDto.OrderName)} is required.");

        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage($"{nameof(OrderDto.CustomerId)} is required.");

        RuleFor(x => x.Order.OrderItems)
            .NotEmpty()
            .WithMessage($"{nameof(OrderDto.OrderItems)} should not be empty.");
    }
}