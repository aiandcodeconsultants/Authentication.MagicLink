using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Authentication.MagicLink.Services;

public class RedisUserService : IUserService
{
    private readonly IDistributedCache _cache;

    public RedisUserService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var userJson = await _cache.GetStringAsync(userId);
        return userJson == null ? null : JsonSerializer.Deserialize<User>(userJson);
    }

    public async Task<User> CreateUserAsync(string email)
    {
        var user = new User { Id = Guid.NewGuid().ToString(), Email = email };
        var userJson = JsonSerializer.Serialize(user);

        // Set the user data with an expiration time
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        await _cache.SetStringAsync(user.Id, userJson, options);

        return user;
    }
}
