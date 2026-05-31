namespace rms_backend.Features.Auth.Commands.Login;

public record LoginRequest(
    string Email,
    string Password
);
