using NUnit.Framework;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong.NTests
{
	[TestFixture()]
	public class TileTests
	{
		[Test()]
		public void ReferenceEqualityTest()
		{
			var list = TileFactory.GetTiles("11m", true).ToList();
			var firstTile = list[0];
			var secondTile = list[1];
			list.Remove(secondTile); 
			// if it's reference comparison, it should remove the second tile. 
			// if it's value comparison, it'll remove the first occurance, which is the first tile, this will make the assertion fail
			Assert.IsTrue(ReferenceEquals(list[0], firstTile));
		}

		[Test()]
		public void test()
		{
			var tiles = TileFactory.GetTiles("11m", true).ToList();
			var list = new List<Tile>(tiles);
			Assert.IsTrue(ReferenceEquals(tiles.First(), list.First()));
		}
	}
}
