using HotChocolate;
using HotChocolate.Data;
using Social.Infrastructure.Data;

namespace Social.Core.Query;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Person> GetUsers([Service] SocialContext context) => context.People;
}