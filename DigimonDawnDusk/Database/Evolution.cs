namespace DigimonDawnDusk.Database;

public class Evolution
{
	public int EvolutionId { get; set; }
	public string Requirements { get; set; } = default!;

	public virtual Digimon From { get; set; } = default!;
	public virtual Digimon To { get; set; } = default!;
}