using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;

namespace JapaneseMahjong
{
	public class Tile : DependencyObject
	{
		public int Value { get; set; } // only 1-9
		public Suit Suit { get; set; }
		public bool IsRed { get; set; } // only for 5m, 5p, 5s
		public Tile()
		{

		}
		public Tile(int value, Suit type, bool isRed = false) : this()
		{
			Value = value;
			Suit = type;
			IsRed = isRed;
		}
		public int SortCode => 2 * (Value + (int)Suit) + (IsRed ? 1 : 0);
		public override string ToString()
		{
			return (IsRed ? 0 : Value).ToString() + Suit.ToString().ToLower()[0];
		}
		public Tile Copy()
		{
			return new Tile(Value, Suit, IsRed);
		}

		public bool SameAs(Tile tile) => Value == tile.Value && Suit == tile.Suit;
	}
	public enum Suit
	{
		Man = 0,
		Pin = 10, 
		Sou = 20, 
		Honor = 30,// 1-4:E,S,W,N; 5-7: Wh,G,R
	}
}
