using Domain.UseCases.CompleteUserRegistration;
using Domain.UseCases.RegisterUser;
using Domain.UseCases.ResetEmail;
using Domain.UseCases.RetrieveUser;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Handlers;

public static class UserHandlers
{
    public static void MapUserHandlers(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/user").WithOpenApi();

        userGroup.MapGet("/{email}", RetrieveUserHandler)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("RetrieveUser");
        
        userGroup.MapPost("/", RegisterUserHandler)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser");
        
        userGroup.MapPost("/{email}/complete/", CompleteUserRegistrationHandler)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("CompleteRegistration");
        
        userGroup.MapPost("/{email}/reset/", ResetEmailHandler)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("ResetEmail");
        
    }

    private static async Task<Results<Ok<UserRetrievedResponse>, ProblemHttpResult>> RetrieveUserHandler(string email, IRetrieveUserUseCase useCase)
    {
        try
        {
            var user = await useCase.RetrieveUser(email);
            
            var response = new UserRetrievedResponse(user.Id, user.Email, user.IsRegistered, user.CreatedAt, user.RecoveryCodes, user.OtpCode);
            
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"User not found: {ex.Message}", statusCode: StatusCodes.Status404NotFound);
        }
        
    }

    private static async Task<Results<Created, ProblemHttpResult>> RegisterUserHandler(RegistrationRequest request, IRegisterUserUseCase useCase)
    {
        try
        {
            await useCase.RegisterUser(request.Email);
            
            return TypedResults.Created();
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"Could not register user: {ex.Message}", statusCode: StatusCodes.Status400BadRequest);
        }
        
    }

    private static async Task<Results<Ok<string>, ProblemHttpResult>> CompleteUserRegistrationHandler(string email, CompleteRegistrationRequest request,
        ICompleteUserRegistrationUseCase useCase)
    {
        try
        {
            var completionResult = await useCase.CompleteRegistration(email, request.Code);

            if (completionResult is RegistrationCompletionResult.RegistrationFailure)
            {
                return TypedResults.Problem($"Registration completion failed",
                    statusCode: StatusCodes.Status400BadRequest);
            }
            
            return TypedResults.Ok("Registration complete");
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"Invalid OTP code: {ex.Message}", statusCode: StatusCodes.Status400BadRequest);
        }
    }

    private static async Task<Results<Ok<string>, ProblemHttpResult>> ResetEmailHandler(string email, ResetEmailRequest request,
        IResetEmailUseCase useCase)
    {
        try
        {
            await useCase.ResetEmail(email, request.newEmail, request.recoveryCode);
            
            return TypedResults.Ok("Email successfully reset, OTP code has been sent for reconfirmation");
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"Could not reset email: {ex.Message}", statusCode: StatusCodes.Status400BadRequest);
        }
    }
}