using System.Collections;

namespace DigimonDawnDusk.Components;

public partial class MultiSelectItems<T>(IEnumerable<T> items) : ICollection<MultiSelectItem<T>>, IReadOnlyCollection<MultiSelectItem<T>>
{
	public List<MultiSelectItem<T>> MultiSelectItemsList { get; } = [.. items.Select(x => new MultiSelectItem<T>(x)).Distinct().OrderBy(x => x.Value)];

	public HashSet<T> SelectedValues() =>
		[.. MultiSelectItemsList
			.Where(x => x.IsSelected)
			.Select(x => x.Value)];

	public int Count => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).Count;

	public bool IsReadOnly => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).IsReadOnly;

	public void Add(MultiSelectItem<T> item) => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).Add(item);
	public void Clear() => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).Clear();
	public bool Contains(MultiSelectItem<T> item) => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).Contains(item);
	public void CopyTo(MultiSelectItem<T>[] array, int arrayIndex) => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).CopyTo(array, arrayIndex);
	public IEnumerator<MultiSelectItem<T>> GetEnumerator() => ((IEnumerable<MultiSelectItem<T>>)MultiSelectItemsList).GetEnumerator();
	public bool Remove(MultiSelectItem<T> item) => ((ICollection<MultiSelectItem<T>>)MultiSelectItemsList).Remove(item);
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)MultiSelectItemsList).GetEnumerator();
}
