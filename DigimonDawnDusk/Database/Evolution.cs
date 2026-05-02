namespace DigimonDawnDusk.Database;

public class Evolution : IDescriptor
{
	public int EvolutionId { get; set; }
	public string Requirements { get; set; } = default!;

	public virtual Digimon From { get; set; } = default!;
	public virtual Digimon? DNAWith { get; set; }

	public virtual Digimon To { get; set; } = default!;

	public string Name => $"{From} -> {To}";

	public string Descriptor => $"{(DNAWith == null ? "" : $"With {DNAWith} ")}{Requirements}";

	public override bool Equals(object? obj) =>
		obj is Evolution evolution &&
		EqualityComparer<Digimon>.Default.Equals(From, evolution.From) &&
		EqualityComparer<Digimon?>.Default.Equals(DNAWith, evolution.DNAWith) &&
		EqualityComparer<Digimon>.Default.Equals(To, evolution.To);

	public override int GetHashCode() => HashCode.Combine(From, DNAWith, To);

	public override string? ToString() => $"{From} -> {To}";
}