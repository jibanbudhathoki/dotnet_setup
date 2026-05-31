using rms_backend.Domain.Entities;

namespace rms_backend.Features.Auth.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
