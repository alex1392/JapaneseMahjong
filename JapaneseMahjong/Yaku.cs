using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Yaku
	{
		public static bool IsWin(IList<Tile> hand)
		{
			throw new NotImplementedException();
		}

		// exceptions:
		// 7 pairs, thirteen orphans
		public static IEnumerable<IEnumerable<Group>> GetValidHands(IEnumerable<Tile> hand)
		{
			results = new List<IEnumerable<Group>>();
			// make a sorted dict for hand
			var sortedHand = hand.OrderBy(t => t.SortCode).ToList();
			var i = 0;
			while (i < sortedHand.Count) {
				var tile = sortedHand[i];
				var count = sortedHand.Skip(i).TakeWhile(t => t == tile).Count();
				// check if can form a pair
				if (count >= 2) {
					var pair = sortedHand.GetRange(i, 2);
					var list = new List<Group> { new Group(pair) };
					sortedHand.RemoveRange(i, 2);
					Decompose(sortedHand, list);
					sortedHand.InsertRange(i, pair);
				}
				i += count;
			}
			return results;
		}

		private static IEnumerable<IEnumerable<Group>> results;
		private static void Decompose(List<Tile> hand, List<Group> list)
		{
			if (hand.Count == 0) {
				results = results.Append(new List<Group>(list));
				return;
			}
			else if (hand.Count < 3) {
				return;
			}

			var tile0 = hand[0];
			var count = hand.TakeWhile(t => t == hand.First()).Count();
			if (count >= 3) { // if the tile can form a triplet
				var triplet = hand.GetRange(0, 3);
				var group = new Group(triplet);
				hand.RemoveRange(0, 3);
				list.Add(group);
				Decompose(hand, list);
				list.Remove(group);
				hand.InsertRange(0, triplet);
			}

			// check if it can form a sequence
			// check it's not honor tile
			if (tile0.Suit != Suit.Honor &&
				// check there are two other different tiles
				hand.FirstOrDefault(t => t != tile0) is Tile tile1 &&
				hand.FirstOrDefault(t => t != tile0 && t != tile1) is Tile tile2 &&
				// check the other tiles are in the same suit
				tile0.Suit == tile1.Suit && tile0.Suit == tile2.Suit &&
				// check the other tiles have consequtive values
				tile0.Value + 1 == tile1.Value && tile0.Value + 2 == tile2.Value) {
				
				var i0 = hand.IndexOf(tile0);
				var i1 = hand.IndexOf(tile1);
				var i2 = hand.IndexOf(tile2);
				hand.Remove(tile0);
				hand.Remove(tile1);
				hand.Remove(tile2);
				var sequence = new List<Tile>
				{
					tile0, tile1, tile2
				};
				var group = new Group(sequence);
				list.Add(group);
				Decompose(hand, list);
				list.Remove(group);
				hand.Insert(i0, tile0);
				hand.Insert(i1, tile1);
				hand.Insert(i2, tile2);
			}
		}

		public static bool CheckAllSimples(IEnumerable<Group> validHand)
		{
			return validHand.Select(g => g.Tiles)
				.Aggregate((aggre, next) => aggre.Concat(next))
				.All(t => t.Value != 1 && t.Value != 9 && t.Suit != Suit.Honor);
		}

		public static bool CheckNoCall(IEnumerable<Group> validHand)
		{
			return validHand.All(g => !(g is OpenGroup));
		}

		public static bool CheckDoubleSequences(IEnumerable<Group> validHand)
		{
			var sequences = validHand.Where(g => g.Type == GroupType.Sequence);
			return CheckNoCall(validHand) &&
				sequences.Distinct().ToList().Count() < sequences.Count();
		}
	}
}
