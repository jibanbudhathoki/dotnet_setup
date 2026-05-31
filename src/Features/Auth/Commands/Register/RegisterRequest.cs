namespace rms_backend.Features.Auth.Commands.Register;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword
);
