using CommanderGQL.Data;
using CommanderGQL.GraphQL.DataLoader;
using CommanderGQL.Models;

namespace CommanderGQL.GraphQL.Platforms;

public class PlatformType : ObjectType<Platform>
{
    protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
    {
        descriptor.Description("Represents any software platform");

        descriptor.Field(p => p.LicenseKey).Ignore();

        descriptor.Field(p => p.Commands)
            .ResolveWith<Resolvers>(r => r.GetCommandsAsync(default!, default!))
            .Description("List of available commands for this platform");
    }

    private class Resolvers
    {
        public async Task<IEnumerable<Command>?> GetCommandsAsync(
            [Parent] Platform platform,
            CommandByPlatformDataLoader commandByPlatformDataLoader)
        {
            return await commandByPlatformDataLoader.LoadAsync(platform.Id);
        }
    }
}