using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DigimonDawnDusk.Components;

public partial class MultiSelectItems<T>(IEnumerable<T> items) :
	IDictionary<T, bool>, IReadOnlyDictionary<T, bool>
	where T : notnull
{
	public bool this[T key] { get => ((IDictionary<T, bool>)MultiSelectItemsList)[key]; set => ((IDictionary<T, bool>)MultiSelectItemsList)[key] = value; }

	public Dictionary<T, bool> MultiSelectItemsList { get; } = new Dictionary<T, bool>(
			 [.. items.Select(x => new KeyValuePair<T, bool>(x, true)).Distinct().OrderBy(x => x.Value)]
		);

	public ICollection<T> Keys => ((IDictionary<T, bool>)MultiSelectItemsList).Keys;

	public ICollection<bool> Values => ((IDictionary<T, bool>)MultiSelectItemsList).Values;

	public int Count => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).Count;

	public bool IsReadOnly => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).IsReadOnly;

	IEnumerable<T> IReadOnlyDictionary<T, bool>.Keys => ((IReadOnlyDictionary<T, bool>)MultiSelectItemsList).Keys;

	IEnumerable<bool> IReadOnlyDictionary<T, bool>.Values => ((IReadOnlyDictionary<T, bool>)MultiSelectItemsList).Values;

	public void Add(T key, bool value) => ((IDictionary<T, bool>)MultiSelectItemsList).Add(key, value);
	public void Add(KeyValuePair<T, bool> item) => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).Add(item);
	public void Clear() => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).Clear();
	public bool Contains(KeyValuePair<T, bool> item) => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).Contains(item);
	public bool ContainsKey(T key) => ((IDictionary<T, bool>)MultiSelectItemsList).ContainsKey(key);
	public void CopyTo(KeyValuePair<T, bool>[] array, int arrayIndex) => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).CopyTo(array, arrayIndex);
	public IEnumerator<KeyValuePair<T, bool>> GetEnumerator() => ((IEnumerable<KeyValuePair<T, bool>>)MultiSelectItemsList).GetEnumerator();
	public bool Remove(T key) => ((IDictionary<T, bool>)MultiSelectItemsList).Remove(key);
	public bool Remove(KeyValuePair<T, bool> item) => ((ICollection<KeyValuePair<T, bool>>)MultiSelectItemsList).Remove(item);
	public bool TryGetValue(T key, [MaybeNullWhen(false)] out bool value) => ((IDictionary<T, bool>)MultiSelectItemsList).TryGetValue(key, out value);
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)MultiSelectItemsList).GetEnumerator();
}
