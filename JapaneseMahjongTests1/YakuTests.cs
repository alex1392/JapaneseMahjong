using NUnit.Framework;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JapaneseMahjong.Tests
{
	[TestFixture()]
	public class YakuTests
	{
		[TestCase("2m2m2m3m3m3m4m4m4m5m5m", ExpectedResult = "2m2m 2m3m4m 3m4m5m 3m4m5m\n2m2m2m 3m3m3m 4m4m4m 5m5m\n2m3m4m 2m3m4m 2m3m4m 5m5m")]
		[TestCase("1h2m2m2m3m3m3m4m4m4m5m5m", ExpectedResult = "")]
		public string GetValidHandsTest(string tileStr)
		{
			var tiles = TileFactory.GetTiles(tileStr);
			var winHands = Yaku.GetValidHands(tiles);

			return string.Join('\n', winHands.Select(hand => hand.GetString()));
		}

		[TestCase("123m 123m 234m 345m", ExpectedResult = true)]
		[TestCase("123m 22m 234m 345m", ExpectedResult = false)]
		public bool CheckDoubleSequencesTest(string groupsString)
		{
			var groups = TileFactory.GetGroups(groupsString, true);
			return Yaku.CheckDoubleSequences(groups);
		}
	}
}