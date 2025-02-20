using BCrypt.Net;

namespace TaskPlannerAPI.Helpers;

public static class PasswordHasher
{
    /// <summary>
    /// Hashes a password using BCrypt.
    /// </summary>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a hashed password.
    /// </summary>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}