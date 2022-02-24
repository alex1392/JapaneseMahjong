using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideSelfCallState : IGameState
	{
		public async Task<IGameState> UpdateAsync(Game game)
		{
			var actions = game.ActivePlayer.CheckSelfCall(game.Wall);
			if (actions.Keys.All(key => key == SelfCallType.None)) {
				return new DecideDiscardState();
			}
			var action = await game.ActivePlayer.DecideSelfCallAsync(actions.Keys);
			switch (action) {
				default:
				case SelfCallType.None:
					return new DecideDiscardState();
				case SelfCallType.Tsumo:
					return new EndState();
				case SelfCallType.Kan:
					Debug.Assert(actions[action].Count > 0);
					if (actions[action].Count > 1) {
						return new DecidetKanTileState(actions[action], this);
					}
					return new CloseKanState(actions[action][0]);
				case SelfCallType.Riichi:
					return new DecideRiichiState(actions[action], this);
			}
		}
	}
}
