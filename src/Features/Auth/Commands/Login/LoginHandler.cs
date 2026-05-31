using rms_backend.Features.Auth.Services;
using rms_backend.Infrastructure.Repositories;
using rms_backend.Shared.Exceptions;
using rms_backend.Shared.Helpers;

namespace rms_backend.Features.Auth.Commands.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedException("Invalid email or password.");

        if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return new LoginResponse(token, user.FullName, user.Email, user.Role);
    }
}
