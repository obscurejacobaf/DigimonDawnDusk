using Microsoft.EntityFrameworkCore;

namespace DigimonDawnDusk.Database;

public partial class DigimonDbContext : DbContext
{
	public DbSet<Move> Moves { get; set; }
	public DbSet<Digimon> Digimon { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}
}
