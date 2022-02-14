using Microsoft.VisualStudio.TestTools.UnitTesting;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;

namespace JapaneseMahjong.Tests
{
	[TestClass()]
	public class OpenGroupTests
	{
		[TestMethod()]
		public void AddedQuadTest()
		{
			var tile = new Tile(1, TileType.Man);
			var tiles = new List<Tile>() { tile, tile.Copy(), tile.Copy() }.ToTiles();
			var group = new OpenGroup(GroupType.Triplet, tiles, 0);
			group.AddedQuad(tile.Copy());
			Assert.IsTrue(group.Type == GroupType.OpenQuad && group.Tiles.Count == 4 && group.ObtainIndex == 0);
		}
	}
}