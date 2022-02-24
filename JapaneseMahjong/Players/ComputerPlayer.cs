using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class ComputerPlayer : PlayerBase
	{
		public ComputerPlayer(int id) : base(id)
		{
		}

		public override Task<bool> ConfirmRiichiAsync(Tile tile)
		{
			throw new NotImplementedException();
		}

		public override Task<CallType> DecideCallAsync()
		{
			throw new NotImplementedException();
		}

		public override Task<Tile> DecideDiscardTileAsync()
		{
			throw new NotImplementedException();
		}

		public override Task<Tile> DecideKanTileAsync(IEnumerable<Tile> list)
		{
			throw new NotImplementedException();
		}

		public override Task<Tile> DecideRiichiTileAsync(IEnumerable<Tile> list)
		{
			throw new NotImplementedException();
		}

		public override Task<SelfCallType> DecideSelfCallAsync(ICollection<SelfCallType> options)
		{
			throw new NotImplementedException();
		}
	}
}
