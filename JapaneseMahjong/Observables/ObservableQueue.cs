using System.Collections.Generic;
using System.Collections.Specialized;

namespace JapaneseMahjong
{
	public class ObservableQueue<TItem> : Queue<TItem>, INotifyCollectionChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public ObservableQueue()
		{

		}
		public ObservableQueue(IEnumerable<TItem> collection)
		{
			foreach (var item in collection) {
				base.Enqueue(item);
			}
		}
		public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}
		public new virtual void Clear()
		{
			base.Clear();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		public new virtual void Enqueue(TItem item)
		{
			base.Enqueue(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}
		public new virtual TItem Dequeue()
		{
			var item = base.Dequeue();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));
			return item;
		}
	}
}
