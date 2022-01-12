using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext: DbContext
    {
        private static string ConnectionString = "Server=barrel.itcollege.ee;User Id=student;Password=Student.Pass.1;Database=student_mashaf_database_v3;MultipleActiveResultSets=true";
        
        public DbSet<GameConfig> GameConfigs { get; set; } = default!;
        public DbSet<GameSave> SavedGames { get; set; } = default!;

        // not recommended - do not hardcode DB conf!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}