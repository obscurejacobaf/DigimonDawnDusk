using DigimonDawnDusk.Database;

namespace DigimonDawnDusk.Components;

public class EvolutionPath
{
	public List<Move> MovesGained { get; set; } = [];
	public List<Digimon> Nodes { get; set; } = [];
	public override string ToString() =>
		$"{(MovesGained.Count == 0 ? "" : $"{string.Join(", ", MovesGained)}: ")}{string.Join(" -> ", Nodes.Select(x => x.Name))}";
}
