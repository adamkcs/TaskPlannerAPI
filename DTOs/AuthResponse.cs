namespace TaskPlannerAPI.DTOs;

/// <summary>
/// Represents the response containing the JWT token.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
}
