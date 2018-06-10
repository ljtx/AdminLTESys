using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ambiel.EntityFrameworkCore
{
    public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<AmbielDbContext>
    {
        public AmbielDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AmbielDbContext>();
            builder.UseMySQL("server=localhost;database=TestDb2;user=root;password=888888;");
            return new AmbielDbContext(builder.Options);
        }
    }
}