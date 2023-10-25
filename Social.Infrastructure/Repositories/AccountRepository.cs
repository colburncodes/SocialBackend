using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Social.Core.Requests;
using Social.Infrastructure.Data;
using Social.Infrastructure.Exceptions;
using Social.Infrastructure.Interfaces;
using User = Social.Core.Responses.User;

namespace Social.Infrastructure.Repositories;

public class AccountRepository : IAccountRespository
{
    private readonly SocialContext _dbContext;
    private ILogger<AccountRepository> _logger;

    public AccountRepository(SocialContext dbContext, ILogger<AccountRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<User> RegisterUserAsync(Register register)
    {
        var userExists = await UserExistAsync(register);
        if (userExists)
        {
            throw new UserExistException("User already exists!");
        }
        
        // TODO: CREATE ACCOUNT METHOD WITH REGISTER OBJ
        
        // TODO: CREATE PASSHASH METHOD TO COMPUTE THE SALT AND HASH
        
        var user = new User()
        {
            Bio = "Bio",
            Email = register.Email,
            Image = "No Image",
            UserName = register.UserName,
            Token = "No Token"
        };

        // TODO: HANDLE DB CONTEXT
        // await _dbContext.Accounts.AddAsync(user);
        // await _dbContext.SaveChangesAsync();

        return user;
    }

    private async Task<bool> UserExistAsync(Register register)
    {
        var result = await _dbContext.Accounts
            .AnyAsync(a => a.Email == register.Email || a.User.UserName == register.UserName);
        return result;
    }
}