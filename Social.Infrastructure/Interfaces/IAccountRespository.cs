using Social.Core.Requests;
using Social.Core.Responses;

namespace Social.Infrastructure.Interfaces;

public interface IAccountRespository
{
    Task<User> RegisterUserAsync(Register register);
    Task<User> LoginUserAsync(Login reqUser);
}