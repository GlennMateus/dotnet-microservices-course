public record UpdateOrderCommand(OrderDto Order) 
    : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator 
    : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty();
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