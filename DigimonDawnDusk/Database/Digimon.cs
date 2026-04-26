namespace DigimonDawnDusk.Database;

public partial class Digimon
{
	public int DigimonId { get; set; }

	public string Name { get; set; } = default!;
	public TypeEnum Type { get; set; }
	public SpeciesEnum Species { get; set; }
	public AttributeEnum Alignment { get; set; }
	public AttributeEnum Weakness { get; set; }
	public string? Dwelling { get; set; }

	public virtual List<Trait> Traits { get; set; } = [];
	public virtual List<Move> Moves { get; set; } = [];
	public virtual List<Evolution> FromEvolutions { get; set; } = [];
	public virtual List<Evolution> ToEvolutions { get; set; } = [];

	public override bool Equals(object? obj) => obj is Digimon digimon && DigimonId == digimon.DigimonId;
	public override int GetHashCode() => HashCode.Combine(DigimonId);
	public override string ToString() => Name;
}
