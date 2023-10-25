using Social.Core.Requests;
using Social.Core.Responses;

namespace Social.Infrastructure.Interfaces;

public interface IAccountRespository
{
    User RegisterUser(Register register);
}