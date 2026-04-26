namespace DigimonDawnDusk.Database;

public class Evolution
{
	public int EvolutionId { get; set; }
	public string Requirements { get; set; } = default!;

	public virtual Digimon From { get; set; } = default!;
	public virtual Digimon To { get; set; } = default!;

	public override bool Equals(object? obj) =>
		obj is Evolution evolution &&
		EqualityComparer<Digimon>.Default.Equals(From, evolution.From) &&
		EqualityComparer<Digimon>.Default.Equals(To, evolution.To);

	public override int GetHashCode() => HashCode.Combine(From, To);
}