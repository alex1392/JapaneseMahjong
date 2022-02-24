using NUnit.Framework;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace JapaneseMahjong.NTests
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

		[TestCase("2223334445m", ExpectedResult = "23456m")]
		[TestCase("1112345678999m", ExpectedResult = "123456789m")]
		public string GetReadyTilesTest(string tileStr)
		{
			var hand = TileFactory.GetTiles(tileStr, true);
			var readyDicts = Yaku.GetReadyTiles(hand);
			foreach (var (tiles, groups) in readyDicts) {
				Debug.WriteLine($"Tiles: {tiles.GetString(true)} / Hand: {string.Join(' ', groups.Select(g => g.GetString(true)))}");
			}
			return readyDicts.Keys.SelectMany(tiles => tiles).Distinct().OrderBy(t => t.SortCode).GetString(true);
		}
	}
}