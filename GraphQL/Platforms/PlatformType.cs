using CommanderGQL.Data;
using CommanderGQL.Models;

namespace CommanderGQL.GraphQL.Platforms;

public class PlatformType : ObjectType<Platform>
{
    protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
    {
        descriptor.Description("Represents any software platform");

        descriptor.Field(p => p.LicenseKey).Ignore();

        descriptor.Field(p => p.Commands)
            .ResolveWith<Resolvers>(p => p.GetCommands(default!, default!)) 
            .Description("List of available commands for this platform");
    }

    private class Resolvers
    {
        public IQueryable<Command> GetCommands([Parent] Platform platform, [Service] AppDbContext context)
        {
            return context.Commands
                .Where(p => p.PlatformId == platform.Id);
        }
    }
}