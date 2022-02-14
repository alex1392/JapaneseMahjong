using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class TilesFactory
	{
		public static Tiles GetFullSet()
		{
			var tiles = new Tiles();
			var types = Enum.GetValues(typeof(TileType)).Cast<TileType>();
			foreach (var type in types.Where(t => t != TileType.Honor)) {
				for (var value = 1; value <= 9; value++) {
					for (var i = 0; i < 3; i++) {
						tiles.Add(new Tile(value, type));
					}
					tiles.Add(new Tile(value, type, value == 5));
				}
			}
			for (var value = 1; value <= 7; value++) {
				for (var i = 0; i < 4; i++) {
					tiles.Add(new Tile(value, TileType.Honor));
				}
			}
			return tiles;
		}

		public static GameTiles Deal()
		{
			var set = GetFullSet();
			set.Shuffle();
			var p0 = set.Skip(13 * 0).Take(13);
			var p1 = set.Skip(13 * 1).Take(13);
			var p2 = set.Skip(13 * 2).Take(13);
			var p3 = set.Skip(13 * 3).Take(13);
			var players = new List<IEnumerable<Tile>>
			{
				p0, p1, p2, p3
			};
			var wall = set.Skip(13 * 4).Take(70);
			var sea = set.TakeLast(14);
			return (players, wall, sea);
		}
	}

	public struct GameTiles
	{
		public IList<IEnumerable<Tile>> PlayerTiles;
		public IEnumerable<Tile> Wall;
		public IEnumerable<Tile> Sea;

		public GameTiles(IList<IEnumerable<Tile>> playerTiles, IEnumerable<Tile> wall, IEnumerable<Tile> sea)
		{
			PlayerTiles = playerTiles;
			Wall = wall;
			Sea = sea;
		}

		public override bool Equals(object obj)
		{
			return obj is GameTiles other &&
				   EqualityComparer<IList<IEnumerable<Tile>>>.Default.Equals(PlayerTiles, other.PlayerTiles) &&
				   EqualityComparer<IEnumerable<Tile>>.Default.Equals(Wall, other.Wall) &&
				   EqualityComparer<IEnumerable<Tile>>.Default.Equals(Sea, other.Sea);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(PlayerTiles, Wall, Sea);
		}

		public void Deconstruct(out IList<IEnumerable<Tile>> playerTiles, out IEnumerable<Tile> wall, out IEnumerable<Tile> sea)
		{
			playerTiles = PlayerTiles;
			wall = Wall;
			sea = Sea;
		}

		public static implicit operator (IList<IEnumerable<Tile>> PlayerTiles, IEnumerable<Tile> Wall, IEnumerable<Tile> Sea)(GameTiles value)
		{
			return (value.PlayerTiles, value.Wall, value.Sea);
		}

		public static implicit operator GameTiles((IList<IEnumerable<Tile>> PlayerTiles, IEnumerable<Tile> Wall, IEnumerable<Tile> Sea) value)
		{
			return new GameTiles(value.PlayerTiles, value.Wall, value.Sea);
		}
	}
}
