using Microsoft.EntityFrameworkCore;
using functions.Entity;

namespace functions.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<PdfData> PdfData { get; set; }
    }
}
