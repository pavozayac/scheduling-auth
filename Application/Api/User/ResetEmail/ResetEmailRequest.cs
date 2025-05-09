namespace Application.Handlers;

public record ResetEmailRequest(string newEmail, string recoveryCode);