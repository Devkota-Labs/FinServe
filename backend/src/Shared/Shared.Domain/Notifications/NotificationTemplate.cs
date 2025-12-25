namespace Shared.Domain.Notifications;
public sealed class NotificationTemplate
{
    public NotificationTemplateKey Key { get; init; }

    public string TitleTemplate { get; init; } = default!;
    public string MessageTemplate { get; init; } = default!;

    public string EmailTemplateName { get; init; } = default!;
}
