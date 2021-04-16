using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chad.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChadDb>
    {
        public ChadDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChadDb>();
            optionsBuilder.UseSqlite("Data Source=chad.db");

            return new ChadDb(optionsBuilder.Options);
        }
    }
}