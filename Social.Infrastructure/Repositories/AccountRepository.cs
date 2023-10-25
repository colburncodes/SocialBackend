using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Social.Core.Requests;
using Social.Infrastructure.Data;
using Social.Infrastructure.Exceptions;
using Social.Infrastructure.Interfaces;
using User = Social.Core.Responses.User;

namespace Social.Infrastructure.Repositories;

public class AccountRepository : IAccountRespository
{
    private readonly SocialContext _dbContext;
    private readonly IMapper _mapper;
    private ILogger<AccountRepository> _logger;
    private IConfiguration _configuration;

    public AccountRepository(SocialContext dbContext, IMapper mapper, IConfiguration configuration, ILogger<AccountRepository> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<User> RegisterUserAsync(Register register)
    {
        var userExists = await UserExistAsync(register);
        if (userExists)
        {
            throw new UserExistException("User already exists!");
        }
        
        var account = await CreateAccountAsync(register);
        var user = CreateUser(account);
        user.Token = CreateToken(user);
        // TODO: HANDLE DB CONTEXT
        // await _dbContext.Accounts.AddAsync(user);
        // await _dbContext.SaveChangesAsync();

        return user;
    }

    private string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.Email)
            }),
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private User CreateUser(Account account)
    {
        var user = new User();
        _mapper.Map(account, user);
        _mapper.Map(account.Person, user);
        return user;
    }

    private async Task<Account> CreateAccountAsync(Register register)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(register.Password, out passwordHash, out passwordSalt);
        var result = await _dbContext.Accounts.AddAsync(new Account()
        {
            Email = register.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Person = new Data.Person()
            {
                UserName = register.UserName
            }
        });
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    private void CreatePasswordHash(string registerPassword, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerPassword));
        }
    }

    private async Task<bool> UserExistAsync(Register register)
    {
        var result = await _dbContext.Accounts
            .AnyAsync(a => a.Email == register.Email || a.Person.UserName == register.UserName);
        return result;
    }
}