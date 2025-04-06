using System.Threading.Tasks;
using CommanderGQL.Data;
using CommanderGQL.GraphQL.DataLoader;
using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.GraphQL.Commands;

public class CommandType : ObjectType<Command>
{
    protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
    {
        descriptor.Description("Represents any executable command");

        descriptor.Field(c => c.Platform)
            .ResolveWith<Resolvers>(c => c.GetPlatformAsync(default!, default!))
            .Description("Represent the platform associated with the command");
    }

    private class Resolvers
    {
        public async Task<Platform?> GetPlatformAsync(
            [Parent] Command command,
            PlatformByIdDataLoader platformByIdDataLoader)
        {
            return await platformByIdDataLoader.LoadAsync(command.PlatformId);
        }
    }
}