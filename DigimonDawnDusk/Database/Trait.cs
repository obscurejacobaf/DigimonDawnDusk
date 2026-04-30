namespace DigimonDawnDusk.Database;

public partial class Trait : IDescriptor, IComparable<Trait>
{
	public int TraitId { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public short Tier { get; set; }

	public virtual List<Digimon> Digimon { get; set; } = [];

	private string TierName => field ??= $"{Tier}: {Name}";
	string IDescriptor.Name => TierName;
	public string Descriptor => $"{Tier}: {Description}";

	public int CompareTo(Trait? other) => TierName.CompareTo(other?.TierName) * -1;
	public override bool Equals(object? obj) => obj is Trait trait && TraitId == trait.TraitId;
	public override int GetHashCode() => HashCode.Combine(TraitId);
}
