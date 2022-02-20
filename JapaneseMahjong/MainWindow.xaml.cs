using JapaneseMahjong.Controls;
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
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public Game Game { get; private set; } = new Game();
		public MainWindow()
		{
			InitializeComponent();
			mainGrid.DataContext = this;
			Loaded += MainWindow_Loaded;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged(string propertyName) 
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Game.RunAsync();
		}

		private void Tile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var tileControl = sender as TileControl;
			Game.Players[0].DiscardTile?.TrySetResult(tileControl.Tile);
		}
	}
}
