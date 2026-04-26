using Microsoft.EntityFrameworkCore;

namespace DigimonDawnDusk.Database;

public partial class DigimonDbContext : DbContext
{
	public DbSet<Digimon> Digimon { get; set; }
	public DbSet<Evolution> Evolutions { get; set; }
	public DbSet<Move> Moves { get; set; }
	public DbSet<Trait> Traits { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Evolution>()
			.HasOne(x => x.From)
			.WithMany(e => e.FromEvolutions);

		modelBuilder.Entity<Evolution>()
			.HasOne(x => x.To)
			.WithMany(e => e.ToEvolutions);
	}
}
