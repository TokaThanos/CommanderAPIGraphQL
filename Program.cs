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

        // Get DB_PASSWORD from environment variables
        string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrEmpty(dbPassword))
        {
            Log.Error("DB_PASSWORD environment variable is not set. Exiting application.");
            return; // Exit program if DB_PASSWORD is missing
        }

        // Read connection string and replace placeholder
        string? connectionString = builder.Configuration.GetConnectionString("CommanderConnectionString")
            ?.Replace("${DB_PASSWORD}", dbPassword);

        builder.Services.AddDbContextPool<AppDbContext>(opt => opt.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information));

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
