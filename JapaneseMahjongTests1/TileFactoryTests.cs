using NUnit.Framework;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong.Tests
{
	[TestFixture()]
	public class TileFactoryTests
	{
		[Test()]
		public void GetFullSetTest()
		{
			var set = TileFactory.GetFullSet();
			var setString = set.GetString();
			Debug.WriteLine(setString);
			var expected = "1m1m1m1m2m2m2m2m3m3m3m3m4m4m4m4m5m5m5m0m6m6m6m6m7m7m7m7m8m8m8m8m9m9m9m9m" + "1p1p1p1p2p2p2p2p3p3p3p3p4p4p4p4p5p5p5p0p6p6p6p6p7p7p7p7p8p8p8p8p9p9p9p9p" + "1s1s1s1s2s2s2s2s3s3s3s3s4s4s4s4s5s5s5s0s6s6s6s6s7s7s7s7s8s8s8s8s9s9s9s9s" + "1h1h1h1h2h2h2h2h3h3h3h3h4h4h4h4h5h5h5h5h6h6h6h6h7h7h7h7h";
			Assert.AreEqual(expected, setString);
			Assert.IsTrue(set.Count() == 4 * (9 * 3 + 7));
		}

		[Test()]
		public void GetTilesTest()
		{
			var expected = "1m1m1m2m3m4m5m5m6m7m8m9m9m9m";
			var tiles = TileFactory.GetTiles(expected);
			var actual = tiles.GetString();
			Assert.AreEqual(expected, actual);
		}

		[TestCase("1m2m3m 4p4p4p", ExpectedResult = "1m2m3m 4p4p4p")]
		public string GetGroupsTest(string groupsString)
		{
			var groups = TileFactory.GetGroups(groupsString);
			return groups.GetString();
		}

		[TestCase("123m 444p", ExpectedResult = "123m 444p")]
		public string GetGroupsTest_Compact(string groupsString)
		{
			var groups = TileFactory.GetGroups(groupsString, true);
			return groups.GetString(true);
		}
	}
}