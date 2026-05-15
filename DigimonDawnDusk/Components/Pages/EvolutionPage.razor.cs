using DigimonDawnDusk.Database;
using Microsoft.EntityFrameworkCore;

namespace DigimonDawnDusk.Components.Pages;

public partial class EvolutionPage(IDbContextFactory<DigimonDbContext> Factory)
{
	private int TargetDigimonId = default!;
	private int MoveOneId = default!;
	private int MoveTwoId = default!;
	private int MoveThreeId = default!;
	private int MoveFourId = default!;

	private List<Digimon> AllDigimon = default!;
	private Dictionary<int, Move> AllMoves = default!;
	private EvolutionPath? ShortestPath = null;

	protected override async Task OnInitializedAsync()
	{
		using var context = await Factory.CreateDbContextAsync();
		AllDigimon = await context.Digimon
			.OrderBy(x => x.Name)
			.ToListAsync();

		AllMoves = await context.Moves
			.Include(x => x.Digimon)
				.ThenInclude(x => x.FromEvolutions)
					.ThenInclude(x => x.To)
			.Include(x => x.Digimon)
				.ThenInclude(x => x.ToEvolutions)
					.ThenInclude(x => x.From)
			.Where(x => x.LevelGained != 0)
			.OrderBy(x => x.Name)
			.ToDictionaryAsync(x => x.MoveId);

		TargetDigimonId = AllDigimon.First().DigimonId;
		MoveOneId = AllMoves.First().Key;
		MoveTwoId = MoveOneId;
		MoveThreeId = MoveOneId;
		MoveFourId = MoveOneId;
	}

	private void CalculateEvolution()
	{
		var target = AllDigimon.First(x => x.DigimonId == TargetDigimonId);

		var goalMoves = new HashSet<Move>
		{
			AllMoves[MoveOneId],
			AllMoves[MoveTwoId],
			AllMoves[MoveThreeId],
			AllMoves[MoveFourId]
		};

		// No need to look for moves if our target mon learns normally.
		var targetMoves = goalMoves.Where(x => target.Moves.Contains(x)).ToList();
		if (targetMoves.Count > 0)
			goalMoves.ExceptWith(target.Moves);

		// Group the mons by number of goal moves they have, then get the max number of moves.
		var monLookup = AllDigimon.ToLookup(x => x.Moves.Count(m => goalMoves.Contains(m)))
			.MaxBy(x => x.Key);
		if (monLookup == null)
			return;

		// Our starting nodes are the mons with the most moves.
		var startMons = monLookup.ToHashSet();
		var shortestPath = FindShortestPath(startMons, goalMoves, target);

		if (shortestPath != null)
			ShortestPath = shortestPath;
		else
			ShortestPath = null;
	}

	private static EvolutionPath? FindShortestPath(HashSet<Digimon> startNodes, HashSet<Move> targetMoves, Digimon target)
	{
		var validPaths = new List<EvolutionPath>();
		var queue = new PriorityQueue<EvolutionPath, double>();

		foreach (var start in startNodes)
		{
			var owned = start.Moves
				.Where(targetMoves.Contains)
				.ToHashSet();

			var p = new EvolutionPath([new(start, owned, null)], new() { [start] = 1 }, [.. targetMoves.Except(owned)]);
			queue.Enqueue(p, p.Priority);
		}

		while (queue.Count > 0)
		{
			var path = queue.Dequeue();
			var current = path.Nodes[^1];

			if (path.Priority > path.Visited[current.Digimon])
				continue;

			if (path.SeekTarget)
			{
				// As soon as this returns true, we've found our shortest path.
				if (current.Digimon.Equals(target))
				{
					return path;
				}
			}
			else
			{
				// Check the the current mon has any of the required moves.
				var newMoves = current.Digimon.Moves
					.Where(path.RequiredMoves.Contains)
					.ToArray();

				// If so, remove the learned moves from the path and clear the visited list so
				// we can begin searching for the next moves/target. This lets us go backwards if needed.
				if (newMoves.Length > 0)
				{
					current.LearnedMoves.UnionWith(newMoves);
					path.RequiredMoves = [.. path.RequiredMoves.Except(newMoves).ToHashSet()];
					path.Visited = new() { [current.Digimon] = 1 };
				}
			}

			// Build a new Evolution Path for each neighbor and add them to the queue.
			foreach (var neighbor in GetNeighbors(current.Digimon))
			{
				var p = new EvolutionPath([.. path.Nodes, neighbor], path.Visited, path.RequiredMoves);
				if (!path.Visited.TryGetValue(neighbor.Digimon, out var existingCost) || existingCost > p.Priority)
				{
					path.Visited[neighbor.Digimon] = p.Priority;
					queue.Enqueue(p, p.Priority);
				}
			}
		}

		return null;
	}

	private static IEnumerable<EvolutionNode> GetNeighbors(Digimon d)
	{
		foreach (var e in d.ToEvolutions.Where(x => x.AllowBackwards))
			yield return new(e.From, [], e);

		foreach (var e in d.FromEvolutions.Where(x => x.AllowForwards))
			yield return new(e.To, [], e);
	}
}
