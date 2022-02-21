using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;

namespace JapaneseMahjong
{
	public struct Tile : IEquatable<Tile>
	{
		public int Value { get; private set; } // only 1-9
		public Suit Suit { get; private set; }
		public bool IsRed { get; private set; } // only for 5m, 5p, 5s
		public int SortCode { get; private set; }

		public Tile(int value, Suit type, bool isRed = false)
		{
			Value = value;
			Suit = type;
			IsRed = isRed;
			SortCode = 2 * (Value + (int)Suit) + (IsRed ? 1 : 0);
		}
		public override string ToString()
		{
			return (IsRed ? 0 : Value).ToString() + Suit.ToString().ToLower()[0];
		}

		#region Implement IEquatable
		public override bool Equals(object obj)
		{
			return obj is Tile && Equals((Tile)obj);
		}

		public bool Equals(Tile other)
		{
			return other != null &&
				   Value == other.Value &&
				   Suit == other.Suit;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Suit);
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

	/// <summary>
	/// Values are used for <see cref="Tile.SortCode"/>
	/// </summary>
	public enum Suit
	{
		None = 0,
		Man = 10,
		Pin = 20, 
		Sou = 30, 
		Honor = 40,// 1-4:E,S,W,N; 5-7: Wh,G,R
	}
}
