namespace rms_backend.Features.Auth.Commands.Register;

public record RegisterResponse(
    Guid UserId,
    string FullName,
    string Email,
    string Role
);
