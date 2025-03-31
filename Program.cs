using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using GraphQL.Server.Ui.Voyager;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL;

public class Program
{
    public static void Main(string[] args) 
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContextPool<AppDbContext>(opt => opt.UseSqlServer
            (builder.Configuration.GetConnectionString("CommanderConnectionString")));
        
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddProjections();

        var app = builder.Build();

        app.MapGraphQL();

        app.UseGraphQLVoyager("/ui/voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });

        app.Run();
    }
}


