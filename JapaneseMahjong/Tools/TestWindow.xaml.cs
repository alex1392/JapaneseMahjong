using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JapaneseMahjong
{
	[AddINotifyPropertyChangedInterface]
	/// <summary>
	/// Interaction logic for TestWindow.xaml
	/// </summary>
	public partial class TestWindow : Window
	{
		public TestWindow()
		{
			InitializeComponent();
			mainGrid.DataContext = this;
			Loaded += TestWindow_Loaded;
		}

		private async void TestWindow_Loaded(object sender, RoutedEventArgs e)
		{
			for (int i = 1; i < 10; i++) {
				await Task.Delay(1000);
				Tiles.Add(new Tile(i, Suit.Man));
			}
		}

		public Tile Tile { get; set; } = new Tile(5, Suit.Man);
		public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();
	}
}
