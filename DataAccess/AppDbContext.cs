using angelasFunctionApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace angelasFunctionApp2.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Products> Products { get; set; }
}