namespace ShipmentService.Domain.Constants;

public static class ShipmentStatusConstants
{
    public const int Draft = 1;
    public const int Pending = 2;
    public const int InTransit = 3;
    public const int Completed = 4;

    public static readonly Dictionary<int, string> StatusNames = new()
    {
        { Draft, "Draft" },
        { Pending, "Pending" },
        { InTransit, "In Transit" },
        { Completed, "Completed" }
    };
}