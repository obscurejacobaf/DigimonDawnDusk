namespace DigimonDawnDusk.Database;

public partial class Trait
{
	public int TraitId { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public short Tier { get; set; }

	public virtual List<Digimon> Digimon { get; set; } = [];

	public override bool Equals(object? obj) => obj is Trait trait && TraitId == trait.TraitId;
	public override int GetHashCode() => HashCode.Combine(TraitId);
}
