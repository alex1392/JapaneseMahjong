using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Tile : IEquatable<Tile>
	{
		public int Value { get; set; } // only 1-9
		public TileType Type { get; set; }
		public bool IsRed { get; set; } // only for 5m, 5p, 5s

		public Tile(int value, TileType type, bool isRed = false)
		{
			Value = value;
			Type = type;
			IsRed = isRed;
		}
		public int GetOrderCode()
		{
			return 2*(Value + (int)Type) + (IsRed ? 1 : 0);
		}
		public override string ToString()
		{
			return (IsRed ? 0 : Value).ToString() + Type.ToString().ToLower()[0];
		}
		public Tile Copy()
		{
			return new Tile(Value, Type, IsRed);
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
				   Type == other.Type;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Type);
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
	public enum TileType
	{
		Man = 0,
		Pin = 10, 
		Sou = 20, 
		Honor = 30,// 1-4:E,S,W,N; 5-7: Wh,G,R
	}
}
