
using Microsoft.EntityFrameworkCore;
 
namespace AnalisisWebsite.Models
{
    public class ValuesTablesContext : DbContext
    {
        public DbSet<TableValue> TableValues { get; set; }
        public ValuesTablesContext(DbContextOptions<ValuesTablesContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}