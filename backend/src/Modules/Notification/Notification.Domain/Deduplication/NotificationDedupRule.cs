namespace Notification.Domain.Deduplication;

public sealed class NotificationDedupRule
{
    public bool Enabled { get; init; }

    /// <summary>
    /// Dedup window (e.g. 10 min, 24 hrs)
    /// </summary>
    public TimeSpan Window { get; init; }

    /// <summary>
    /// Function to extract dedup key from event data
    /// </summary>
    public Func<Dictionary<string, object?>, string?> DedupKeyResolver { get; init; }
        = _ => null;
}
