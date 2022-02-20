using Microsoft.VisualStudio.TestTools.UnitTesting;
using JapaneseMahjong;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace JapaneseMahjong.Tests
{
	[TestClass()]
	public class GameTests
	{
		[TestMethod()]
		public void DealTest()
		{
			var game = new Game();
			game.Deal();
			var set = game.Players.Select(p => p.Hand)
				.Cast<IEnumerable<Tile>>()
				.Aggregate((all, next) => all.Concat(next))
				.Concat(game.Wall)
				.Concat(game.Sea)
				.OrderBy(t => t.SortCode);
			var setString = set.GetString();
			var expected = "1m1m1m1m2m2m2m2m3m3m3m3m4m4m4m4m5m5m5m0m6m6m6m6m7m7m7m7m8m8m8m8m9m9m9m9m" + "1p1p1p1p2p2p2p2p3p3p3p3p4p4p4p4p5p5p5p0p6p6p6p6p7p7p7p7p8p8p8p8p9p9p9p9p" + "1s1s1s1s2s2s2s2s3s3s3s3s4s4s4s4s5s5s5s0s6s6s6s6s7s7s7s7s8s8s8s8s9s9s9s9s" + "1h1h1h1h2h2h2h2h3h3h3h3h4h4h4h4h5h5h5h5h6h6h6h6h7h7h7h7h";
			Assert.AreEqual(expected, setString);
		}
	}
}