using Social.Core.Requests;
using Social.Core.Responses;
using Social.Infrastructure.Data;

namespace Social.Infrastructure.Interfaces;

public interface IAccountRespository
{
    Task<User> RegisterUserAsync(Register register);
    Task<User> LoginUserAsync(Login reqUser);
    Task<User> GetCurrentUserAsync();
    Task<Account> GetLoggedInUserAsync();
}