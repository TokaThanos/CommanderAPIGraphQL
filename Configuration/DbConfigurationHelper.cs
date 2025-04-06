using CommanderGQL.Data;
using Microsoft.EntityFrameworkCore;

namespace CommanderGQL.Configuration
{
    public static class DbConfigurationHelper
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = GetConnectionString(configuration);
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(dbPassword))
            {
                throw new InvalidOperationException("DB_PASSWORD environment variable is not set.");
            }

            string? rawConnectionString = configuration.GetConnectionString("CommanderConnectionString");

            if (string.IsNullOrEmpty(rawConnectionString))
            {
                throw new InvalidOperationException("CommanderConnectionString is not set in configuration.");
            }

            return rawConnectionString.Replace("${DB_PASSWORD}", dbPassword);
        }
    }
}