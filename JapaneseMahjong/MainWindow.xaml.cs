using JapaneseMahjong;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JapaneseMahjong
{
	[AddINotifyPropertyChangedInterface]
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Game Game { get; private set; } = new GameStub();

		public ObservableCollection<SelfCallType> SelfCallOptions { get; private set; } = new ObservableCollection<SelfCallType>();
		public TaskCompletionSource<SelfCallType> DecideSelfCall { get; private set; }

		public ObservableCollection<Tile> TileOptions { get; private set; } = new ObservableCollection<Tile>();

		public HumanPlayer MainPlayer { get; private set; }
		public TaskCompletionSource<Tile> DecideKanTile { get; private set; }

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			IdentifyMainPlayer();
			Game.Deal();
			await Game.RunAsync();
		}

		private void IdentifyMainPlayer()
		{
			if (!Game.Players.Any(p => p is HumanPlayer)) {
				return;
			}
			MainPlayer = Game.Players.First(p => p is HumanPlayer) as HumanPlayer;
			HumanPlayerEventsSubscription();
		}

		private void HumanPlayerEventsSubscription()
		{
			MainPlayer.ShowSelfCallOptions += async (_, options) =>
			{
				foreach (var option in options) {
					SelfCallOptions.Add(option);
				}
				DecideSelfCall = new TaskCompletionSource<SelfCallType>();
				var selfCall = await DecideSelfCall.Task;
				MainPlayer?.DecideSelfCall?.TrySetResult(selfCall);
			};
			MainPlayer.ShowKanTileOptions += async (_, tiles) =>
			{
				foreach (var t in tiles) {
					TileOptions.Add(t);
				}
				DecideKanTile = new TaskCompletionSource<Tile>();
				var tile = await DecideKanTile.Task;
				MainPlayer?.DecideKanTile?.TrySetResult(tile);
			};
		}

		private void DiscardTile(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is TileControl tileControl)) {
				return;
			}

			MainPlayer.DecideDiscardTile?.TrySetResult(tileControl.Tile);
		}

		private void SelectSelfCall(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is Label label) ||
				!(label.DataContext is SelfCallType selfCall)) {
				return;
			}
			DecideSelfCall?.TrySetResult(selfCall);
			SelfCallOptions.Clear();
		}

		private void SelectKanTile(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is Label label) ||
				!(label.DataContext is Tile tile)) {
				return;
			}
			DecideKanTile?.TrySetResult(tile);
			TileOptions.Clear();
		}
	}
}
