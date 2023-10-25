using Microsoft.Extensions.Logging;
using Social.Core.Requests;
using Social.Core.Responses;
using Social.Infrastructure.Interfaces;

namespace Social.Infrastructure.Repositories;

public class AccountRepository : IAccountRespository
{
    private ILogger<AccountRepository> _logger;

    public AccountRepository(ILogger<AccountRepository> logger)
    {
        _logger = logger;
    }

    public User RegisterUser(Register register)
    {
        var user = new User()
        {
            Bio = "Bio",
            Email = register.Email,
            Image = "No Image",
            UserName = register.UserName,
            Token = "No Token"
        };

        return user;
    }
}