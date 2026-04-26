namespace DigimonDawnDusk.Database
{
	public partial class Move
	{
		public int MoveId { get; set; }

		public AttributeEnum Attribute { get; set; }
		public int AttackTimes { get; set; }
		public string Name { get; set; } = "";
		public int Effect { get; set; }
		public string Range { get; set; } = "";
		public string Description { get; set; } = "";
		public bool Fixed { get; set; }
		public int MpCost { get; set; }
		public int LevelGained { get; set; }

		public string GainedBy => field ??= string.Join(", ", Digimon.Select(x => x.Name));

		private int? effectZones;
		public int EffectZones => effectZones ??= Range.Count(x => x == 'O');

		private int? singleTargetEffect;
		public int SingleTargetEffect => singleTargetEffect ??= Effect * AttackTimes;

		private int? aoeEffect;
		public int AOEEffect => aoeEffect ??= SingleTargetEffect * EffectZones;

		public virtual List<Digimon> Digimon { get; set; } = [];

		public override string ToString() => Name;
		public override bool Equals(object? obj) => obj is Move move && MoveId == move.MoveId;
		public override int GetHashCode() => HashCode.Combine(MoveId);
	}
}
