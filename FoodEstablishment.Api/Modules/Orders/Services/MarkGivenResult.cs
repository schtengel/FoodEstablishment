namespace FoodEstablishment.Api.Modules.Orders.Services;

public abstract record MarkGivenResult
{
    private MarkGivenResult()
    {
    }

    public sealed record Success : MarkGivenResult;

    public sealed record NotFound : MarkGivenResult;

    public sealed record Rejected(string Message) : MarkGivenResult;
}