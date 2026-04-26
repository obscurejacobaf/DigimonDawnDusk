using Microsoft.EntityFrameworkCore;

namespace DigimonDawnDusk.Database;

public sealed class DigimonDbInitializer(IDbContextFactory<DigimonDbContext> factory)
{
	public void Initialize()
	{
		using var context = factory.CreateDbContext();
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		InitTraits(context);
		InitDigimon(context);
		InitNormalEvolution(context);
		InitMoves(context);
	}

	private static void InitTraits(DigimonDbContext context)
	{
		using var stream = new FileStream(@"C:\Users\theja\source\repos\DigimonWorldDD\DigimonDawnDusk\Data\Traits.txt", FileMode.Open);
		using var reader = new StreamReader(stream);

		short tier = 1;
		var traits = new List<Trait>();

		while (true)
		{
			var line = reader.ReadLine();
			if (line == null)
				break;

			if (line == string.Empty)
			{
				tier = 1;
				continue;
			}

			if (line.StartsWith('τ'))
			{
				tier = short.Parse(line[1..]);
				continue;
			}

			var parts = line.Split(" - ");
			traits.Add(new()
			{
				Name = parts[0],
				Description = parts[1],
				Tier = tier
			});

			tier++;
		}

		context.Traits.AddRange(traits);
		context.SaveChanges();
	}

	private static void InitDigimon(DigimonDbContext context)
	{
		using var stream = new FileStream(@"C:\Users\theja\source\repos\DigimonWorldDD\DigimonDawnDusk\Data\Digimon.txt", FileMode.Open);
		using var reader = new StreamReader(stream);

		var traits = context.Traits.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
		var digimon = new Digimon();
		var digimonList = new List<Digimon>();

		while (true)
		{
			var line = reader.ReadLine();
			if (line == null)
				break;

			if (line == string.Empty || line.StartsWith("Digimon No.:"))
				continue;

			if (line.Contains("------------------"))
			{
				digimonList.Add(digimon);
				digimon = new();
				continue;
			}

			if (line.StartsWith("Digimon Name: "))
			{
				digimon.Name = line[14..];
				continue;
			}

			if (line.StartsWith("Type: "))
			{
				digimon.Type = Enum.Parse<TypeEnum>(line[6..]);
				continue;
			}

			if (line.StartsWith("Species: "))
			{
				digimon.Species = Enum.Parse<SpeciesEnum>(line[9..]);
				continue;
			}

			if (line.StartsWith("Elemental Alignment: "))
			{
				digimon.Resistance = Enum.Parse<AttributeEnum>(line[21..]);
				continue;
			}

			if (line.StartsWith("Elemental Weakness: "))
			{
				digimon.Weakness = Enum.Parse<AttributeEnum>(line[20..]);
				continue;
			}

			if (line.StartsWith("Dwelling: "))
			{
				var d = line[10..];
				digimon.Dwelling = d == "Not Available" ? string.Empty : d;
				continue;
			}

			if (line.StartsWith("Traits:"))
			{
				while (true)
				{
					line = reader.ReadLine();
					if (line == null || !line.StartsWith("- "))
						break;

					digimon.Traits.Add(traits[line[2..]]);
				}

				continue;
			}

			throw new Exception($"Invalid line {line}");
		}

		context.Digimon.AddRange(digimonList);
		context.SaveChanges();
	}

	private static void InitNormalEvolution(DigimonDbContext context)
	{
		using var stream = new FileStream(@"C:\Users\theja\source\repos\DigimonWorldDD\DigimonDawnDusk\Data\NormalEvolution.txt", FileMode.Open);
		using var reader = new StreamReader(stream);

		var digimon = context.Digimon.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
		var evolutions = new List<Evolution>();

		while (true)
		{
			var line = reader.ReadLine();
			if (string.IsNullOrEmpty(line))
				break;

			var parts = line.Split(" -> ");
			var parts2 = parts[1].Split(": ");

			evolutions.Add(new()
			{
				From = digimon[parts[0]],
				To = digimon[parts2[0]],
				Requirements = parts2[1]
			});
		}

		context.Evolutions.AddRange(evolutions);
		context.SaveChanges();
	}

	private static void InitMoves(DigimonDbContext context)
	{
		using var stream = new FileStream(@"C:\Users\theja\source\repos\DigimonWorldDD\DigimonDawnDusk\Data\Moves.txt", FileMode.Open);
		using var reader = new StreamReader(stream);

		var workingAttribute = AttributeEnum.Holy;
		var workingAttackTimes = 0;

		var digimon = context.Digimon.ToDictionary(x => x.Name);
		var moves = new List<Move>();
		var move = new Move();

		while (true)
		{
			var line = reader.ReadLine();
			if (string.IsNullOrEmpty(line))
				break;

			if (Enum.TryParse<AttributeEnum>(line, out var attribute))
			{
				move.Attribute = attribute;
				workingAttribute = attribute;
				continue;
			}
			if (Constants.IsEffectCountRow().IsMatch(line))
			{
				move.AttackTimes = int.Parse(line[0].ToString());
				workingAttackTimes = move.AttackTimes;
				continue;
			}
			if (line == "Buff")
			{
				workingAttackTimes = 1;
				move.AttackTimes = 1;
				continue;
			}
			if (line == "Special Techniques")
				continue;

			if (line.StartsWith("Effect & Range: "))
			{
				var match = Constants.GetEffectAndRange().Match(line);
				_ = int.TryParse(match.Groups[1].Value, out var effect);
				move.Effect = effect;
				move.Range = match.Groups[2].Value.Replace('X', '_').Trim('_');
				move.Fixed = match.Groups[3].Value == "Fixed";
			}
			else if (line.StartsWith("MP Cost: "))
			{
				move.MpCost = int.Parse(line[9..]);
			}
			else if (line.StartsWith("Description: "))
			{
				move.Description = line[13..];
			}
			else if (line.StartsWith("Level Gained: "))
			{
				move.LevelGained = int.Parse(line[14..]);
			}
			else if (line.StartsWith("Attribute: "))
			{
				move.Attribute = Enum.Parse<AttributeEnum>(line[11..]);
			}
			else if (line.StartsWith("Gained By: "))
			{
				var gainedBy = line[11..].Split(", ");
				foreach (var mon in gainedBy)
				{
					move.Digimon.Add(digimon[mon]);
				}

				// Need to fix a single edge case where the title is also the name of an element.
				if (string.IsNullOrEmpty(move.Name))
					move.Name = "Thunder";

				moves.Add(move);
				move = new Move
				{
					Attribute = workingAttribute,
					AttackTimes = workingAttackTimes
				};
			}
			else
			{
				move.Name = line;
			}
		}

		context.Moves.AddRange(moves);
		context.SaveChanges();
	}
}
