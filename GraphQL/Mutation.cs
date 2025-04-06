using CommanderGQL.Data;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.Models;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL
{
    public class Mutation
    {
        public async Task<AddPlatformPayload> AddPlatformAsync(
            AddPlatformInput input,
            [Service] IDbContextFactory<AppDbContext> dbContextFactory,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();

            var platform = new Platform
            {
                Name = input.Name
            };

            context.Platforms.Add(platform);
            await context.SaveChangesAsync();

            await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancellationToken);

            return new AddPlatformPayload(platform);
        }

        public async Task<AddCommandPayload> AddCommandAsync(
            AddCommandInput input,
            [Service] IDbContextFactory<AppDbContext> dbContextFactory)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();

            var command = new Command
            {
                HowTo = input.HowTo,
                CommandLine = input.CommandLine,
                PlatformId = input.PlatformId
            };

            context.Commands.Add(command);
            await context.SaveChangesAsync();

            return new AddCommandPayload(command);
        }
    }
}