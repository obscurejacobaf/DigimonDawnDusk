using DigimonDawnDusk.Database;

namespace DigimonDawnDusk.Components;

public class EvolutionPath
{
	public Move? MoveGained { get; set; }
	public List<Digimon> Nodes { get; set; } = [];
	public override string ToString() =>
		$"{(MoveGained == null ? "" : $"{MoveGained.Name}: ")}{string.Join(" -> ", Nodes.Select(x => x.Name))}";
}
