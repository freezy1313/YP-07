using Microsoft.EntityFrameworkCore;

namespace AgencyEmploymentApp.Data;

public partial class AppDbContext
{
    public const string DefaultConnectionString =
        "Host=localhost;Port=5432;Database=ais_agency_employment;Username=postgres;Password=23082008";

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(DefaultConnectionString);
        }
    }
}
