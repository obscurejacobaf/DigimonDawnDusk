namespace DigimonDawnDusk.Database;

public partial class Digimon
{
	public int DigimonId { get; set; }

	public required string Name { get; set; }

	public virtual List<Digimon> EvolvesFrom { get; set; } = [];
	public virtual List<Digimon> EvolvesTo { get; set; } = [];
	public virtual List<Move> Moves { get; set; } = [];

	public override string ToString() => Name;
}
