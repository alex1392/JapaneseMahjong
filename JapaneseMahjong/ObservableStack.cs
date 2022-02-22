using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace JapaneseMahjong
{
	public class ObservableStack<TItem> : Stack<TItem>, INotifyCollectionChanged 
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public ObservableStack()
		{

		}
		public ObservableStack(IEnumerable<TItem> collection) : this()
		{
			foreach (var item in collection) {
				base.Push(item);
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
		public new virtual void Push(TItem item)
		{
			base.Push(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}
		public new virtual TItem Pop()
		{
			var item = base.Pop();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, Count));
			return item;
		}
	}

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
