namespace Authentication.MagicLink.Interfaces;

/// <summary>An interface for the user service.</summary>
public interface IUserService
{
    /// <summary>Gets a user by id.</summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The user, or null if not matched.</returns>
    Task<User?> GetUserByIdAsync(string userId);

    /// <summary>Creates a user.</summary>
    /// <param name="email">The email address.</param>
    /// <returns>The created user.</returns>
    Task<User> CreateUserAsync(string email);
}