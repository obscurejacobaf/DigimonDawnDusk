using System.Text.RegularExpressions;

namespace DigimonDawnDusk;

public static partial class Constants
{

	[GeneratedRegex(@"^\dx$")]
	public static partial Regex IsEffectCountRow();
	[GeneratedRegex(@"^Effect & Range: ([\d,—]+), \((.....)\) (.*)$")]
	public static partial Regex GetEffectAndRange();
}
