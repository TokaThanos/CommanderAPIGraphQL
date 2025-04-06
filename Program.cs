using CommanderGQL.Configuration;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.DataLoader;
using CommanderGQL.GraphQL.Platforms;
using GraphQL.Server.Ui.Voyager;
using Serilog;

namespace CommanderGQL;

public class Program
{
    public static void Main(string[] args) 
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();

        DbConfigurationHelper.ConfigureDatabase(builder.Services, builder.Configuration);

        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddType<PlatformType>()
            .AddType<CommandType>()
            .AddDataLoader<CommandByPlatformDataLoader>()
            .AddDataLoader<PlatformByIdDataLoader>()
            .AddFiltering()
            .AddSorting()
            .AddInMemorySubscriptions()
            .ModifyRequestOptions(opts => opts.IncludeExceptionDetails = true);

        var app = builder.Build();

        app.UseWebSockets();

        app.MapGraphQL();

        app.UseGraphQLVoyager("/ui/voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });

        app.Run();
    }
}
