namespace rms_backend.Features.Auth.Commands.Login;

public record LoginResponse(
    string Token,
    string FullName,
    string Email,
    string Role
);
