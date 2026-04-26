namespace DigimonDawnDusk.Components;

public sealed class MultiSelectItem<T>(T value)
{
	public T Value { get; set; } = value;
	public bool IsSelected { get; set; } = true;

	public override bool Equals(object? obj) => obj is MultiSelectItem<T> item &&
		EqualityComparer<T>.Default.Equals(Value, item.Value);
	public override int GetHashCode() => HashCode.Combine(Value);
}