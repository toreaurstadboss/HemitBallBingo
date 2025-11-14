using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HemitBallbingoContext>
    {
        public HemitBallbingoContext CreateDbContext(string[] args)
        {
            // Hent config fra appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HemitBallbingoContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new HemitBallbingoContext(optionsBuilder.Options);
        }
    }
}
