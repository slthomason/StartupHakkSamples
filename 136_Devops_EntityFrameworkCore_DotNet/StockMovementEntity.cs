namespace ArticleDevOpsEfCore.Database;

public class StockMovementEntity
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTimeOffset OccurredOn { get; set; }
}