using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideSelfCallState : IGameState
	{
		private readonly IEnumerable<Tile> closeKanTiles;
		private readonly IEnumerable<Tile> addKanTiles;
		private readonly IEnumerable<Tile> riichiTiles;
		private readonly IEnumerable<SelfCallType> options;

		public DecideSelfCallState(bool isTsumo, IEnumerable<Tile> closeKanTiles, IEnumerable<Tile> addKanTiles, IEnumerable<Tile> riichiTiles)
		{
			this.closeKanTiles = closeKanTiles;
			this.addKanTiles = addKanTiles;
			this.riichiTiles = riichiTiles;
			options = new[] { addKanTiles.Any(), closeKanTiles.Any(), riichiTiles.Any(), isTsumo }
			.Select((b, i) => (SelfCallType)(!b ? 0 : i + 1))
			.Where(call => call != SelfCallType.None)
			.Append(SelfCallType.None);
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			var action = await game.ActivePlayer.DecideSelfCallAsync(options);
			return action switch
			{
				SelfCallType.AddKan => new DecideAddKanTileState(addKanTiles, this),
				SelfCallType.CloseKan => new DecideCloseKanTileState(closeKanTiles, this),
				SelfCallType.Riichi => new DecideRiichiState(riichiTiles, this),
				SelfCallType.Tsumo => new EndState(),
				_ => new DecideDiscardState(),
			};
		}
	}
}
