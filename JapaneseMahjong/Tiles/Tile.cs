using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;

namespace JapaneseMahjong
{
	public class Tile
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

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Suit, IsRed, SortCode);
		}

		/// <summary>
		/// Value comparison
		/// </summary>
		public static bool operator ==(Tile left, Tile right)
		{
			if (left is null && right is null) {
				return true;
			}
			if (left is null ^ right is null) {
				return false;
			}
			return left.Value == right.Value &&
				left.Suit == right.Suit;
		}

		public static bool operator !=(Tile left, Tile right)
		{
			return !(left == right);
		}
	}

	public class TileComparer : IEqualityComparer<Tile>
	{
		public bool Equals([AllowNull] Tile x, [AllowNull] Tile y)
		{
			return x == y;
		}

		public int GetHashCode([DisallowNull] Tile obj)
		{
			return obj.GetHashCode();
		}
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
