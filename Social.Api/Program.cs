using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Social.Core.Query;
using Social.Core.Responses;
using Social.Infrastructure.Data;
using Social.Infrastructure.Interfaces;
using Social.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddHttpClient("rest", c => c.BaseAddress = new Uri("http://localhost:5227"));
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup();

builder.Services.AddDbContext<SocialContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.
builder.Services.AddAutoMapper(map =>
{
    map.CreateMap<Account, User>();
    map.CreateMap<Person, User>();
});
builder.Services.AddScoped<IAccountRespository, AccountRepository>();

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = new SerializableError(context.ModelState);
        var result = new UnprocessableEntityObjectResult(new { errors });
        return result;
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{ c.SwaggerDoc("v1", new OpenApiInfo { Title = "Social.Api", Version = "v1" });
    // internal documentation purposes not something for production
    // postman would be a better option.
    var jwtSecuritySchema = new OpenApiSecurityScheme()
    {
        BearerFormat = "JWT",
        Name = "JWT Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "JWT Authorization header using the Bearer scheme.",
        
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    c.AddSecurityDefinition(jwtSecuritySchema.Reference.Id, jwtSecuritySchema);
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {jwtSecuritySchema, Array.Empty<string>()}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social.Api v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();