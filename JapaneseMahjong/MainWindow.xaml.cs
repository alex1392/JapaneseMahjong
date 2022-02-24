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
		public Game Game { get; private set; } = new Game();
		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Game.RunAsync();
		}

		private void DiscardTileFromHand(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is TileControl tileControl)) {
				return;
			}
			if (!(Game.Players[0] is HumanPlayer player)) {
				return;
			}

			player.DecideDiscardTile?.TrySetResult(tileControl.Tile);
		}

		private void DiscardTileFromDraw(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is TileControl tileControl)) {
				return;
			}
			if (!(Game.Players[0] is HumanPlayer player)) {
				return;
			}
			player.DecideDiscardTile?.TrySetResult(tileControl.Tile);
		}
	}
}
