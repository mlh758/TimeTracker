namespace TimeTrack.Server.Repositories;
using TimeTrack.Server.Models;
using TimeTrack.Server.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
public interface IUserRepository
{
    // Creates a user, converting the given plaintext password to a hashed value
    Task<User> Create(User user);
    
    Task<User?> Login(string email, string password);
}
public class UserRepository : IUserRepository
{
    private readonly TimeContext _timeContext;
    public UserRepository(TimeContext context)
    {
        _timeContext = context;
    }
    public async Task<User> Create(User user)
    {
        if (user.Salt is null)
        {
            user.Salt = new byte[8];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(user.Salt);
        }

        var password = user.PasswordSalt + user.Password;
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, password);
        _timeContext.Users.Add(user);
        await _timeContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Login(string email, string password)
    {
        var user = await _timeContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }
        var hasher = new PasswordHasher<User>();
        var match = hasher.VerifyHashedPassword(user, user.Password, user.PasswordSalt + password);
        if (match == PasswordVerificationResult.Success)
        {
            return user;
        }
        return null;
    }
}
