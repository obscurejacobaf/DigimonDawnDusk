using DigimonDawnDusk.Database;

namespace DigimonDawnDusk.Components;

public class EvolutionPath(List<EvolutionNode> nodes, HashSet<Digimon> visited, HashSet<Move> movesRequired)
{
	public bool SeekTarget => RequiredMoves.Count == 0;

	public HashSet<Move> RequiredMoves { get; set; } = movesRequired;
	public HashSet<Digimon> Visited { get; set; } = visited;
	public List<EvolutionNode> Nodes { get; set; } = nodes;
}

public record EvolutionNode(Digimon Digimon, HashSet<Move> LearnedMoves, Evolution? Evolution = null) : IDescriptor
{
	public string Name => Digimon.ToString();
	public bool HasLearnedMoves => LearnedMoves?.Count > 0;
	public string Descriptor => Evolution?.Descriptor ?? "";
}
