using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Tiles : IList<Tile>
	{
		private readonly List<Tile> tiles = new List<Tile>();
		private readonly Random rand = new Random();

		public override string ToString() => string.Join(null, tiles.Select(t => t.ToString()));
		public void InOrder() => tiles.Sort(new Comparison<Tile>((x,y) => x.GetOrderCode() - y.GetOrderCode()));

		public void Shuffle() => tiles.Sort(new Comparison<Tile>((x, y) => rand.Next() - rand.Next()));

		public Tiles() { }

		public Tiles(IEnumerable<Tile> source) : this() => tiles.AddRange(source);

		#region implement IList

		public int Count => tiles.Count;

		public bool IsReadOnly => true;

		public Tile this[int index] { get => tiles[index]; set => tiles[index] = value; }

		public int IndexOf(Tile item) => tiles.IndexOf(item);

		public void Insert(int index, Tile item) => tiles.Insert(index, item);

		public void RemoveAt(int index) => tiles.RemoveAt(index);

		public void Add(Tile item) => tiles.Add(item);

		public void Clear() => tiles.Clear();

		public bool Contains(Tile item) => tiles.Contains(item);

		public void CopyTo(Tile[] array, int arrayIndex) => tiles.CopyTo(array, arrayIndex);

		public bool Remove(Tile item) => tiles.Remove(item);

		public IEnumerator<Tile> GetEnumerator() => tiles.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
		#endregion
	}

	public static class TilesExtensions
	{
		public static Tiles ToTiles(this IEnumerable<Tile> source) => new Tiles(source);
	}
}
