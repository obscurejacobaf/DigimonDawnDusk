using Microsoft.EntityFrameworkCore;

namespace DigimonDawnDusk.Database;

public sealed class DigimonDbInitializer(IDbContextFactory<DigimonDbContext> factory)
{
	public void Initialize()
	{
		using var context = factory.CreateDbContext();
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();

		using var stream = new FileStream(@"C:\Users\theja\source\repos\DigimonWorldDD\DigimonDawnDusk\Data\Moves.txt", FileMode.Open);
		using var reader = new StreamReader(stream);

		var workingAttribute = AttributeEnum.Holy;
		var workingAttackTimes = 0;

		var digimon = new Dictionary<string, Digimon>();
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
					if (!digimon.TryGetValue(mon, out var existingMon))
					{
						existingMon = new Digimon() { Name = mon };
						digimon.Add(mon, existingMon);
					}

					move.Digimon.Add(existingMon);
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
