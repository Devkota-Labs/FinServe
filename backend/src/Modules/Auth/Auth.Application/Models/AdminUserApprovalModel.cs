namespace Auth.Application.Models;

internal sealed record AdminUserApprovalModel(string UserName, string FullName, string UserEmail, Uri ApproveLink);

