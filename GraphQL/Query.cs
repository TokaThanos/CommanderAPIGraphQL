using CommanderGQL.Data;
using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL;

public class Query
{
    public async Task<List<Platform>> GetPlatformsAsync([Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Platforms.ToListAsync();
    }
}