using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Interaction logic for TileSet.xaml
	/// </summary>
	public partial class TileSet : UserControl
	{
		public TileSet()
		{
			InitializeComponent();
			Loaded += TileSet_Loaded;
		}

		private void TileSet_Loaded(object sender, RoutedEventArgs e)
		{
			var suits = Enum.GetValues(typeof(Suit)).Cast<Suit>();
			foreach (var suit in suits.Where(t => t != Suit.Honor)) {
				for (int value = 1; value <= 9; value++) {
					wrapPanel.Children.Add(new TileControl(new Tile(value, suit)));
					if (value == 5) {
						wrapPanel.Children.Add(new TileControl(new Tile(value, suit, true)));
					}
				}
			}
			for (int value = 1; value <= 7; value++) {
				wrapPanel.Children.Add(new TileControl(new Tile(value, Suit.Honor)));
			}
		}
	}
}
