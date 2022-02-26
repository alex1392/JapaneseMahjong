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

		#region Events
		public event EventHandler<IEnumerable<SelfCallType>> ShowSelfCallOptions;
		private void RaiseShowSelfCallOptions(IEnumerable<SelfCallType> options)
		{
			ShowSelfCallOptions?.Invoke(this, options);
		}
		public event EventHandler<IEnumerable<Tile>> ShowKanTileOptions;
		private void RaiseShowTileOptions(IEnumerable<Tile> tiles)
		{
			ShowKanTileOptions?.Invoke(this, tiles);
		}
		#endregion

		public override Task<CallType> DecideCallAsync()
		{
			throw new NotImplementedException();
		}

		public override async Task<Tile> DecideDiscardTileAsync()
		{
			DecideDiscardTile = new TaskCompletionSource<Tile>();
			return await DecideDiscardTile.Task;
		}

		public override async Task<SelfCallType> DecideSelfCallAsync(IEnumerable<SelfCallType> options)
		{
			RaiseShowSelfCallOptions(options);
			DecideSelfCall = new TaskCompletionSource<SelfCallType>();
			return await DecideSelfCall.Task;
		}

		public override async Task<Tile> DecideKanTileAsync(IEnumerable<Tile> tiles)
		{
			RaiseShowTileOptions(tiles);
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
		None, AddKan, CloseKan, Riichi, Tsumo
	}
	/// <summary>
	/// Priority: Ron > Kan > Pon > Chi
	/// </summary>
	public enum CallType
	{
		None, Chi, Pon, Kan, Ron
	}
}
