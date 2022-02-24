using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Yaku
	{
		public static IDictionary<IEnumerable<Tile>, IEnumerable<IGroup>> GetReadyTiles(IEnumerable<Tile> hand)
		{
			var readyDict = new Dictionary<IEnumerable<Tile>, IEnumerable<IGroup>>();
			var sortedHand = hand.OrderBy(t => t.SortCode);

			Decompose(sortedHand, new List<SemiGroup>(), new List<FullGroup>());
			return readyDict;

			void Decompose(IEnumerable<Tile> hand, IEnumerable<SemiGroup> readyGroups, IEnumerable<FullGroup> groups)
			{
				if (hand.Count() == 0) {
					IEnumerable<Tile> tiles;
					if (readyGroups.All(g => g.Type == SemiGroupType.Pair)) {
						tiles = readyGroups.SelectMany(readyGroup => readyGroup.ReadyTiles);
					} else {
						tiles = readyGroups.First(g => g.Type != SemiGroupType.Pair).ReadyTiles;
					}
					readyDict.Add(tiles, groups.Cast<IGroup>().Concat(readyGroups.Cast<IGroup>()));
					return;
				}

				// check for one tile (single ready)
				if (readyGroups.Count() == 0) {
					Decompose(hand.Skip(1), readyGroups.Append(new SemiGroup(hand.Take(1))), groups);
				}

				// check for two tiles (ready groups)
				var tile0 = hand.First();
				var tile1 = hand.FirstOrDefault(t => t != tile0);
				var tile2 = hand.FirstOrDefault(t => t != tile0 && t != tile1);
				var count = hand.TakeWhile(t => t == tile0).Count();
				if (readyGroups.Count() == 0 ||
					readyGroups.Count() == 1 &&
					readyGroups.First().Type != SemiGroupType.Single) {

					SemiGroup readyGroup;
					if (count >= 2) {
						readyGroup = new SemiGroup(hand.Take(2));
						DecomposeReadyGroup(readyGroup);
					}
					if (tile1 != default) {
						readyGroup = new SemiGroup(new[] { tile0, tile1 });
						DecomposeReadyGroup(readyGroup);
					}
					if (tile2 != default) {
						readyGroup = new SemiGroup(new[] { tile0, tile2 });
						DecomposeReadyGroup(readyGroup);
					}
				}

				// check for three tiles (groups)
				if (count >= 3) {
					Decompose(hand.Skip(3), readyGroups, groups.Append(new FullGroup(hand.Take(3))));
				}

				if (tile0.Suit != Suit.Honor &&
					tile1 != default && tile2 != default &&
					tile0.Suit == tile1.Suit && tile0.Suit == tile2.Suit &&
					tile0.Value + 1 == tile1.Value && tile0.Value + 2 == tile2.Value) {
					var handList = hand.ToList();
					foreach (var tile in new[] { tile0, tile1, tile2 }) {
						handList.Remove(tile);
					}
					Decompose(handList, readyGroups, groups.Append(new FullGroup(new List<Tile> { tile0, tile1, tile2 })));
				}

				void DecomposeReadyGroup(SemiGroup readyGroup)
				{
					if (readyGroup.Type != SemiGroupType.None &&
						(readyGroups.Count() == 0 ||
						readyGroups.First().Type == SemiGroupType.Pair ||
						readyGroup.Type == SemiGroupType.Pair)) {
						var handList = hand.ToList();
						foreach (var tile in readyGroup.Tiles) {
							handList.Remove(tile);
						}
						Decompose(handList, readyGroups.Append(readyGroup), groups);
					}
				}
			}
		}


		// exceptions:
		// 7 pairs, thirteen orphans
		public static IEnumerable<IEnumerable<FullGroup>> GetValidHands(IEnumerable<Tile> hand)
		{
			var results = new List<IEnumerable<FullGroup>>();
			var sortedHand = hand.OrderBy(t => t.SortCode);
			var i = 0;
			while (i < sortedHand.Count()) {
				var tile = sortedHand.Skip(i).First();
				var count = sortedHand.Skip(i).TakeWhile(t => t == tile).Count();
				// check if can form a pair
				if (count >= 2) {
					var list = new List<FullGroup> { new FullGroup(sortedHand.Skip(i).Take(2)) };
					Decompose(sortedHand.Take(i).Concat(sortedHand.Skip(i+2)), list);
				}
				i += count;
			}
			return results;

			void Decompose(IEnumerable<Tile> hand, IEnumerable<FullGroup> list)
			{
				if (hand.Count() == 0) {
					results.Add(list);
					return;
				} 
				if (hand.Count() < 3) {
					return;
				}

				var tile0 = hand.First();
				var count = hand.TakeWhile(t => t == hand.First()).Count();
				if (count >= 3) { // if the tile can form a triplet
					Decompose(hand.Skip(3), list.Append(new FullGroup(hand.Take(3))));
				}

				// check if it can form a sequence
				// check it's not honor tile
				var tile1 = hand.FirstOrDefault(t => t != tile0);
				var tile2 = hand.FirstOrDefault(t => t != tile0 && t != tile1);
				if (tile0.Suit != Suit.Honor &&
					// check there are two other different tiles
					tile1 != default && tile2 != default &&
					// check the other tiles are in the same suit
					tile0.Suit == tile1.Suit && tile0.Suit == tile2.Suit &&
					// check the other tiles have consequtive values
					tile0.Value + 1 == tile1.Value && tile0.Value + 2 == tile2.Value) {
					
					var handList = hand.ToList();
					var tiles = new[] { tile0, tile1, tile2 };
					foreach (var tile in tiles) {
						handList.Remove(tile); // remove by value
					}
					Decompose(handList, list.Append(new FullGroup(tiles)));
				}
			}
		}

		public static bool CheckAllSimples(IEnumerable<FullGroup> validHand)
		{
			return validHand.Select(g => g.Tiles)
				.Aggregate((aggre, next) => aggre.Concat(next))
				.All(t => t.Value != 1 && t.Value != 9 && t.Suit != Suit.Honor);
		}

		public static bool CheckDoubleSequences(IEnumerable<FullGroup> validHand)
		{
			var sequences = validHand.Where(g => g.Type == GroupType.Sequence);
			var distinct = sequences.Distinct();
			return distinct.Count() < sequences.Count();
		}
	}
}
