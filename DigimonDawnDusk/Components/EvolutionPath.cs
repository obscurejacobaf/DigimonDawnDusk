using DigimonDawnDusk.Database;

namespace DigimonDawnDusk.Components;

public class EvolutionPath
{
	public List<Move> MovesGained { get; set; } = [];
	public List<EvolutionNode> Nodes { get; set; } = [];
}

public record EvolutionNode(Digimon Digimon, Evolution? Evolution = null) : IDescriptor
{
	public string Name => Digimon.ToString();

	public string Descriptor => Evolution?.Descriptor ?? "";
}
