using rms_backend.Domain.Entities;
using rms_backend.Infrastructure.Repositories;
using rms_backend.Shared.Exceptions;
using rms_backend.Shared.Helpers;

namespace rms_backend.Features.Auth.Commands.Register;

public class RegisterHandler
{
    private readonly IUserRepository _userRepository;

    public RegisterHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterResponse> HandleAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var exists = await _userRepository.ExistsByEmailAsync(request.Email, ct);
        if (exists)
            throw new ConflictException($"A user with email '{request.Email}' already exists.");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = PasswordHelper.Hash(request.Password),
            Role = "User"
        };

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        return new RegisterResponse(user.Id, user.FullName, user.Email, user.Role);
    }
}
