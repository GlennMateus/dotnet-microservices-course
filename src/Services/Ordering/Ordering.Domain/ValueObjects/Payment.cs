namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string CarName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;

    protected Payment() { }
    private Payment(string carName
        , string cardNumber
        , string expiration
        , string cvv
        , int paymentMethod)
    {
        CarName = carName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string carName
        , string cardNumber
        , string expiration
        , string cvv
        , int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(carName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment(carName, cardNumber, expiration, cvv, paymentMethod);
    }
}
