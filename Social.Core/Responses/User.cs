using System.Security.Principal;

namespace Social.Core.Responses;

public class User
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}