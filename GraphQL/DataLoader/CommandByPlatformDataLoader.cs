using CommanderGQL.Data;
using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL.DataLoader;

public class CommandByPlatformDataLoader : BatchDataLoader<int, IEnumerable<Command>>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public CommandByPlatformDataLoader(
        IBatchScheduler batchScheduler,
        IDbContextFactory<AppDbContext> dbContextFactory,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, IEnumerable<Command>>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var commands = await dbContext.Commands
            .Where(c => keys.Contains(c.PlatformId))
            .ToListAsync(cancellationToken);
        
        var result = keys.ToDictionary(
            key => key,
            key => (IEnumerable<Command>)commands
                .Where(c => c.PlatformId == key)
                .ToList()
        );

        return result;
    }
}
