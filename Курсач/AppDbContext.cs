using System.Data.Entity;

namespace Курсач
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Repairs> Repairs { get; set; }

        public DbSet<RepairRequest> RepairRequests { get; set; }

        public AppDbContext() : base("name=LAPTOP-7C3UR1FK\\SQLEXPRESS")
        {
        }
    }
}
