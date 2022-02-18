using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JapaneseMahjong.Controls
{
	/// <summary>
	/// Interaction logic for Tile.xaml
	/// </summary>
	public partial class TileControl : UserControl, INotifyPropertyChanged
	{
		public TileControl()
		{
			InitializeComponent();
			mainGrid.DataContext = this;
		}

		public TileControl(Tile tile) : this()
		{
			Tile = tile;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Tile Tile
		{
			get { return (Tile)GetValue(TileProperty); }
			set { SetValue(TileProperty, value); }
		}
		public static readonly DependencyProperty TileProperty =
			DependencyProperty.Register("Tile", typeof(Tile), typeof(TileControl), new PropertyMetadata(null));

		[DependsOn(nameof(Tile))]
		public ImageSource ImageSource => new BitmapImage(new Uri($"/Resources/{Tile.Suit}{Tile.Value}{(Tile.IsRed ? "-Red" : null)}.png", UriKind.Relative));
	}
}
