
using DirectKeyDashboard.Charting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DirectKeyDashboard.Data {
    public class ApplicationDbContext : DbContext {
        private readonly string _connectionString;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration appConfiguration) : base(options) {
            _connectionString = appConfiguration.GetConnectionString("ApplicationDbContext");
        }

        // public DbSet<SomeEntityType> Entities {get; set;}
        public DbSet<CustomBarChart> CustomBarCharts {get; set;}
        public DbSet<CustomBarChart.CustomBarChartFloatCriterion> BarChartFloatCriteria {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            builder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            // Change table properties, such as relationships, here
            builder.Entity<CustomBarChart>().HasMany(e => e.FloatCriteria).WithOne(e => e.CustomBarChart);
        }
    }
}