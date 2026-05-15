using DigimonDawnDusk.Database;

namespace DigimonDawnDusk.Components;

public class EvolutionPath(List<EvolutionNode> nodes, Dictionary<Digimon, double> visited, HashSet<Move> movesRequired)
{
	public bool SeekTarget => RequiredMoves.Count == 0;

	public HashSet<Move> RequiredMoves { get; set; } = movesRequired;
	public Dictionary<Digimon, double> Visited { get; set; } = visited;
	public List<EvolutionNode> Nodes { get; set; } = nodes;

	public double Priority => Nodes.Count + (Nodes[^1].Evolution?.DNAWith == null ? 0 : 0.5);
}

public record EvolutionNode(Digimon Digimon, HashSet<Move> LearnedMoves, Evolution? Evolution = null) : IDescriptor
{
	public string Name => Digimon.ToString();
	public bool HasLearnedMoves => LearnedMoves?.Count > 0;
	public string Descriptor => Evolution?.Descriptor ?? "";
}
