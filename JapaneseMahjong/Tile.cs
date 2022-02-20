using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;

namespace JapaneseMahjong
{
	public class Tile : IEquatable<Tile>
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

		#region Implement IEquatable
		public override bool Equals(object obj)
		{
			return Equals(obj as Tile);
		}

		public bool Equals(Tile other)
		{
			return other != null &&
				   Value == other.Value &&
				   Suit == other.Suit &&
				   IsRed == other.IsRed;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Suit, IsRed);
		}

		public static bool operator ==(Tile left, Tile right)
		{
			return EqualityComparer<Tile>.Default.Equals(left, right);
		}

		public static bool operator !=(Tile left, Tile right)
		{
			return !(left == right);
		} 
		#endregion
	}

	public class TileSorter : IComparer<Tile>
	{
		public int Compare(Tile x, Tile y) => x.SortCode - y.SortCode;
	}
	public enum Suit
	{
		Man = 0,
		Pin = 10, 
		Sou = 20, 
		Honor = 30,// 1-4:E,S,W,N; 5-7: Wh,G,R
	}
}
