using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NagaisoraFamework
{
	public class NRtype
	{
		public object Value;
		public int Type;
		public string Name;

		public NRtype(object Val, int type, string name)
		{
			Value = Val;
			Type = type;
			Name = name;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object operator +(NRtype a, NRtype b)
		{
			if (a.Type != b.Type)
			{
				throw null;
			}

			switch (a.Type)
			{
				case 0:
					return (int)a.Value + (int)b.Value;
				case 1:
					return (long)a.Value + (long)b.Value;
				case 2:
					return (float)a.Value + (float)b.Value;
				case 3:
					return (double)a.Value + (double)b.Value;
				case 4:
					return null;
				case 5:
					return (string)a.Value + (string)b.Value;
				case 6:
					return (char)a.Value + (char)b.Value;
				case 7:
					return null;
				case 8:
					return (Vector2)a.Value + (Vector2)b.Value;
				case 9:
					return (Vector3)a.Value + (Vector3)b.Value;
				case 10:
					return (Vector6)a.Value + (Vector6)b.Value;
				default:
					return null;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object operator -(NRtype a, NRtype b)
		{
			if (a.Type != b.Type)
			{
				throw null;
			}

			switch (a.Type)
			{
				case 0:
					return (int)a.Value - (int)b.Value;
				case 1:
					return (long)a.Value - (long)b.Value;
				case 2:
					return (float)a.Value - (float)b.Value;
				case 3:
					return (double)a.Value - (double)b.Value;
				case 4:
					return null;
				case 5:
					return null;
				case 6:
					return (char)a.Value - (char)b.Value;
				case 7:
					return (DateTime)a.Value - (DateTime)b.Value;
				case 8:
					return (Vector2)a.Value - (Vector2)b.Value;
				case 9:
					return (Vector3)a.Value - (Vector3)b.Value;
				case 10:
					return (Vector6)a.Value - (Vector6)b.Value;
				default:
					return null;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object operator *(NRtype a, NRtype b)
		{
			if (a.Type != b.Type & a.Type != 8 & a.Type != 9 & a.Type != 10)
			{
				throw null;
			}

			switch (a.Type)
			{
				case 0:
					return (int)a.Value * (int)b.Value;
				case 1:
					return (long)a.Value * (long)b.Value;
				case 2:
					return (float)a.Value * (float)b.Value;
				case 3:
					return (double)a.Value * (double)b.Value;
				case 4:
					return null;
				case 5:
					return null;
				case 6:
					return (char)a.Value * (char)b.Value;
				case 7:
					return null;
				case 8:
					if (a.Type == 8 & b.Type == 8)
					{
						return (Vector2)a.Value * (Vector2)b.Value;
					}
					else if (a.Type == 8)
					{
						return (Vector2)a.Value * (float)b.Value;
					}
					else
					{
						return (float)a.Value * (Vector2)b.Value;
					}
				case 9:
					if (a.Type == 9)
					{
						return (Vector3)a.Value * (float)b.Value;
					}
					else
					{
						return (float)a.Value * (Vector3)b.Value;
					}
				case 10:
					if (a.Type == 10)
					{
						return (Vector6)a.Value * (float)b.Value;
					}
					else
					{
						return (float)a.Value * (Vector6)b.Value;
					}
				default:
					return null;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object operator /(NRtype a, NRtype b)
		{
			if (a.Type != b.Type & a.Type != 8 & a.Type != 9 & a.Type != 10)
			{
				throw null;
			}

			switch (a.Type)
			{
				case 0:
					return (int)a.Value / (int)b.Value;
				case 1:
					return (long)a.Value / (long)b.Value;
				case 2:
					return (float)a.Value / (float)b.Value;
				case 3:
					return (double)a.Value / (double)b.Value;
				case 4:
					return null;
				case 5:
					return null;
				case 6:
					return (char)a.Value / (char)b.Value;
				case 7:
					return null;
				case 8:
					if (a.Type == 8 & b.Type == 8)
					{
						return (Vector2)a.Value / (Vector2)b.Value;
					}
					else if (a.Type == 8)
					{
						return (Vector2)a.Value / (float)b.Value;
					}
					else
					{
						return null;
					}
				case 9:
					if (a.Type == 9)
					{
						return (Vector3)a.Value / (float)b.Value;
					}
					else
					{
						return null;
					}
				case 10:
					if (a.Type == 10)
					{
						return (Vector6)a.Value / (float)b.Value;
					}
					else
					{
						return null;
					}
				default:
					return null;
			}
		}

		public static NRtype operator ++(NRtype a)
		{
			object obj;

			switch (a.Type)
			{
				case 0:
					obj = (int)a.Value + 1;
					break;
				case 1:
					obj = (long)a.Value + 1;
					break;
				case 2:
					obj = (float)a.Value + 1;
					break;
				case 3:
					obj = (double)a.Value + 1;
					break;
				case 4:
					obj = null;
					break;
				case 5:
					obj = null;
					break;
				case 6:
					obj = (char)a.Value + 1;
					break;
				case 7:
					obj = null;
					break;
				case 8:
					if (a.Type == 8)
					{
						obj = (Vector2)a.Value + new Vector2(1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				case 9:
					if (a.Type == 9)
					{
						obj = (Vector3)a.Value + new Vector3(1f, 1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				case 10:
					if (a.Type == 10)
					{
						obj = (Vector6)a.Value + new Vector6(1f, 1f, 1f, 1f, 1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				default:
					obj = null;
					break;
			}

			return new NRtype(obj, a.Type, a.Name);
		}

		public static NRtype operator --(NRtype a)
		{
			object obj;

			switch (a.Type)
			{
				case 0:
					obj = (int)a.Value - 1;
					break;
				case 1:
					obj = (long)a.Value - 1;
					break;
				case 2:
					obj = (float)a.Value - 1;
					break;
				case 3:
					obj = (double)a.Value - 1;
					break;
				case 4:
					obj = null;
					break;
				case 5:
					obj = null;
					break;
				case 6:
					obj = (char)a.Value - 1;
					break;
				case 7:
					obj = null;
					break;
				case 8:
					if (a.Type == 8)
					{
						obj = (Vector2)a.Value - new Vector2(1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				case 9:
					if (a.Type == 9)
					{
						obj = (Vector3)a.Value - new Vector3(1f, 1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				case 10:
					if (a.Type == 10)
					{
						obj = (Vector6)a.Value - new Vector6(1f, 1f, 1f, 1f, 1f, 1f);
					}
					else
					{
						obj = null;
					}
					break;
				default:
					obj = null;
					break;
			}

			return new NRtype(obj, a.Type, a.Name);
		}

		public static bool operator ==(NRtype lhs, NRtype rhs)
		{
			if (lhs.Type != rhs.Type)
			{
				return false;
			}

			if (lhs.Value != rhs.Value)
			{
				return false;
			}

			return true;
		}

		public static bool operator !=(NRtype lhs, NRtype rhs)
		{
			if (lhs.Type == rhs.Type)
			{
				return false;
			}

			if (lhs.Value == rhs.Value)
			{
				return false;
			}

			return true;
		}

		public override bool Equals(object obj)
		{
			return obj is NRtype rtype &&
				   Name == rtype.Name &&
				   EqualityComparer<object>.Default.Equals(Value, rtype.Value) &&
				   Type == rtype.Type;
		}

		public override int GetHashCode()
		{
			int hashCode = 1477810893;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
			hashCode = hashCode * -1521134295 + Type.GetHashCode();
			return hashCode;
		}
	}
}