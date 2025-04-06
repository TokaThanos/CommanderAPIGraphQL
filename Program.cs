using CommanderGQL.Configuration;
using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using GraphQL.Server.Ui.Voyager;
using Microsoft.EntityFrameworkCore;
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
            .AddType<PlatformType>()
            .AddType<CommandType>();

        var app = builder.Build();

        app.MapGraphQL();

        app.UseGraphQLVoyager("/ui/voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });

        app.Run();
    }
}
