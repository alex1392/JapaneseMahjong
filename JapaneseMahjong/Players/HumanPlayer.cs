using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class HumanPlayer : PlayerBase
	{
		public HumanPlayer(int id) : base(id)
		{

		}

		public TaskCompletionSource<Tile> DecideDiscardTile { get; private set; }
		public TaskCompletionSource<SelfCallType> DecideSelfCall { get; private set; }
		public TaskCompletionSource<Tile> DecideKanTile { get; private set; }
		public TaskCompletionSource<Tile> DecideRiichiTile { get; private set; }
		public TaskCompletionSource<bool> ConfirmRiichiTile { get; private set; }

		public override Task<CallType> DecideCallAsync()
		{
			throw new NotImplementedException();
		}

		public override async Task<Tile> DecideDiscardTileAsync()
		{
			DecideDiscardTile = new TaskCompletionSource<Tile>();
			return await DecideDiscardTile.Task;
		}

		public override async Task<SelfCallType> DecideSelfCallAsync(ICollection<SelfCallType> options)
		{
			// TODO: push option list to the main window
			DecideSelfCall = new TaskCompletionSource<SelfCallType>();
			return await DecideSelfCall.Task;
		}

		public override async Task<Tile> DecideKanTileAsync(IEnumerable<Tile> list)
		{
			// TODO: push options to the main window
			DecideKanTile = new TaskCompletionSource<Tile>();
			return await DecideKanTile.Task;
		}

		public override async Task<Tile> DecideRiichiTileAsync(IEnumerable<Tile> list)
		{
			// TODO: push options to the main window
			DecideRiichiTile = new TaskCompletionSource<Tile>();
			return await DecideRiichiTile.Task;
		}

		public override async Task<bool> ConfirmRiichiAsync(Tile tile)
		{
			// TODO: let user peek the ready tiles
			ConfirmRiichiTile = new TaskCompletionSource<bool>();
			return await ConfirmRiichiTile.Task;
		}
	}

	public enum SelfCallType
	{
		None, Kan, Tsumo, Riichi
	}
	public enum CallType
	{
		None, Chi, Pon, Kan
	}
}
