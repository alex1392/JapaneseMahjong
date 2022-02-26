using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	// currently this only draw and discard tiles
	// TODO: incorporate strategy pattern (?) for different play style
	public class ComputerPlayer : PlayerBase
	{
		public ComputerPlayer(int id) : base(id)
		{
		}

		public override async Task<bool> ConfirmRiichiAsync(Tile tile)
		{
			return await Task.Run(() => false);
		}

		public override async Task<CallType> DecideCallAsync()
		{
			return await Task.Run(() => CallType.None);
		}

		public override async Task<Tile> DecideDiscardTileAsync()
		{
			return await Task.Run(() => DrawedTile);
		}

		public override async Task<Tile> DecideKanTileAsync(IEnumerable<Tile> list)
		{
			return await Task.Run(() => list.First());
		}

		public override async Task<Tile> DecideRiichiTileAsync(IEnumerable<Tile> list)
		{
			return await Task.Run(() => list.First());
		}

		public override async Task<SelfCallType> DecideSelfCallAsync(IEnumerable<SelfCallType> options)
		{
			return await Task.Run(() => SelfCallType.None);
		}
	}
}
