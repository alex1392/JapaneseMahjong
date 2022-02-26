using System;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideCallState : IGameState
	{
		public Task<IGameState> UpdateAsync(Game game)
		{
			throw new NotImplementedException();
			foreach (var player in game.Players) {
				var dictionary = player.CheckCall(game.ActivePlayer, game.ActivePlayer.River.Last());
				if (true) {
					// push action list to the user
					
					// wait for the user to decide whether to call

				}
			}
		}
	}
}
