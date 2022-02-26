using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class CheckSelfCallState : IGameState
	{
		public async Task<IGameState> UpdateAsync(Game game)
		{
			var player = game.ActivePlayer;
			var isTsumo = player.CheckTsumo();
			var closeKanTiles = player.CheckCloseKan();
			var addKanTiles = player.CheckAddKan();
			var riichiTiles = player.CheckRiichi(game.Wall);
			
			if (!isTsumo && !closeKanTiles.Any() && !addKanTiles.Any() && !riichiTiles.Any()) {
				return await Task.Run(() => new DecideDiscardState());
			}
			return await Task.Run(() => new DecideSelfCallState(isTsumo, closeKanTiles, addKanTiles, riichiTiles));
		}
	}
}