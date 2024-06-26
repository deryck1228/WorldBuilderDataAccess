using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WorldBuilderDataAccessLib
{
    public class CampaignContextFactory : IDesignTimeDbContextFactory<CampaignContext>
    {
        public CampaignContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            //TODO Abstract out connection string rather than hard code
            var connectionString = "Server=tcp:worldbuildingserver.database.windows.net,1433;Initial Catalog=worldbuildingdb;Persist Security Info=False;User ID=worldbuildingadmin;Password=Stryker1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; //configuration.GetConnectionString("SqlConnectionString");
            Console.WriteLine($"Connection String: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<CampaignContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CampaignContext(optionsBuilder.Options);
        }

    }
}
