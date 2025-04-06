using CommanderGQL.Data;
using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL.DataLoader;

public class PlatformByIdDataLoader : BatchDataLoader<int, Platform>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public PlatformByIdDataLoader(
        IBatchScheduler batchScheduler, 
        IDbContextFactory<AppDbContext> dbContextFactory, 
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, Platform>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        return await context.Platforms
            .Where(p => keys.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}