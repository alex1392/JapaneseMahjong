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
	public partial class Tile : UserControl, INotifyPropertyChanged
	{
		public Tile()
		{
			InitializeComponent();
			mainGrid.DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public int Value { get; set; } // only 1-9
		public TileType Type { get; set; }
		public bool IsRed { get; set; } // only for 5m, 5p, 5s

		[DependsOn(nameof(Value), nameof(Type), nameof(IsRed))]
		public ImageSource ImageSource => new BitmapImage(new Uri($"/Resources/{Type}{Value}{(IsRed ? "-Red" : null)}.png", UriKind.Relative));
	}
}
