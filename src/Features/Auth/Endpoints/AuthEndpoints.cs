using FluentValidation;
using rms_backend.Features.Auth.Commands.Login;
using rms_backend.Features.Auth.Commands.Register;
using rms_backend.Shared.Responses;

namespace rms_backend.Features.Auth.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (
            RegisterRequest request,
            RegisterHandler handler,
            IValidator<RegisterRequest> validator,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
            }

            var result = await handler.HandleAsync(request, ct);
            return Results.Created($"/api/users/{result.UserId}",
                ApiResponse<RegisterResponse>.Ok(result, "User registered successfully."));
        });

        group.MapPost("/login", async (
            LoginRequest request,
            LoginHandler handler,
            IValidator<LoginRequest> validator,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(ApiResponse<object>.Fail("Validation failed.", errors));
            }

            var result = await handler.HandleAsync(request, ct);
            return Results.Ok(ApiResponse<LoginResponse>.Ok(result, "Login successful."));
        });
    }
}
