using NUnit.Framework;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;

namespace JapaneseMahjong.Tests
{
	[TestFixture()]
	public class TilesExtensionsTests
	{
		[TestCase("1m2m3m1p2p3p1s2s3s", false, ExpectedResult = "123m 123p 123s")]
		[TestCase("123m 123p 123s", true, ExpectedResult = "123m 123p 123s")]
		public string GetStringCompactTest(string tileString, bool isCompact)
		{
			var tiles = TileFactory.GetTiles(tileString, isCompact);
			return tiles.GetString(true);
		}
	}
}