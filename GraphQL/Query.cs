using CommanderGQL.Data;
using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL;

public class Query
{
    [UseFiltering]
    [UseSorting]
    public async Task<List<Platform>> GetPlatforms(
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Platforms.ToListAsync();
    }

    [UseFiltering]
    [UseSorting]
    public async Task<List<Command>> GetCommands(
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Commands.ToListAsync();
    }
}
