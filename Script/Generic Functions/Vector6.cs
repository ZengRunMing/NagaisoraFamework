using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine;

namespace NagaisoraFamework
{
	public struct Vector6 : IEquatable<Vector6>, IFormattable
	{
		public const float kEpsilon = 1E-05f;

		public const float kEpsilonNormalSqrt = 1E-15f;

		public float x;
		public float y;
		public float z;

		public float a;
		public float b;
		public float c;

		public static readonly Vector6 zeroVector = new Vector6(0f, 0f, 0f, 0f, 0f, 0f);

		public static readonly Vector6 oneVector = new Vector6(1f, 1f, 1f, 0f, 0f, 0f);

		public static readonly Vector6 upVector = new Vector6(0f, 1f, 0f, 0f, 0f, 0f);

		public static readonly Vector6 downVector = new Vector6(0f, -1f, 0f, 0f, 0f, 0f);

		public static readonly Vector6 leftVector = new Vector6(-1f, 0f, 0f, 0f, 0f, 0f);

		public static readonly Vector6 rightVector = new Vector6(1f, 0f, 0f, 0f, 0f, 0f);

		public static readonly Vector6 forwardVector = new Vector6(0f, 0f, 1f, 0f, 0f, 0f);

		public static readonly Vector6 backVector = new Vector6(0f, 0f, -1f, 0f, 0f, 0f);

		public static readonly Vector6 positiveInfinityVector = new Vector6(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		public static readonly Vector6 negativeInfinityVector = new Vector6(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		public float this[int index]
		{
			get
			{
				return index switch
				{
					0 => x,
					1 => y,
					2 => z,
					3 => a,
					4 => b,
					5 => c,
					_ => throw new IndexOutOfRangeException("Invalid Vector6 index!"),
				};
			}
			set
			{
				switch (index)
				{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					case 2:
						z = value;
						break;
					case 3:
						a = value;
						break;
					case 4:
						b = value;
						break;
					case 5:
						c = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid Vector6 index!");
				}
			}
		}

		public Vector6 normalized => Normalize(this);

		public float magnitude => (float)Math.Sqrt(x * x + y * y + z * z + a * a + b * b + c * c);

		public float sqrMagnitude => x * x + y * y + z * z + a * a + b * b + c * c;

		public static Vector6 zero => zeroVector;

		public static Vector6 one => oneVector;

		public static Vector6 forward => forwardVector;

		public static Vector6 back => backVector;

		public static Vector6 up => upVector;

		public static Vector6 down => downVector;

		public static Vector6 left => leftVector;

		public static Vector6 right => rightVector;

		public static Vector6 positiveInfinity => positiveInfinityVector;

		public static Vector6 negativeInfinity => negativeInfinityVector;

		[Obsolete("Use Vector6.forward instead.")]
		public static Vector6 fwd => new Vector6(0f, 0f, 1f, 0f, 0f, 0f);

		public static Vector6 Slerp(Vector6 a, Vector6 b, float t)
		{
			Slerp_Injected(ref a, ref b, t, out Vector6 ret);
			return ret;
		}

		public static Vector6 SlerpUnclamped(Vector6 a, Vector6 b, float t)
		{
			SlerpUnclamped_Injected(ref a, ref b, t, out Vector6 ret);
			return ret;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void OrthoNormalize2(ref Vector6 a, ref Vector6 b);

		public static void OrthoNormalize(ref Vector6 normal, ref Vector6 tangent)
		{
			OrthoNormalize2(ref normal, ref tangent);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void OrthoNormalize3(ref Vector6 a, ref Vector6 b, ref Vector6 c);

		public static void OrthoNormalize(ref Vector6 normal, ref Vector6 tangent, ref Vector6 binormal)
		{
			OrthoNormalize3(ref normal, ref tangent, ref binormal);
		}

		public static Vector6 RotateTowards(Vector6 current, Vector6 target, float maxRadiansDelta, float maxMagnitudeDelta)
		{
			RotateTowards_Injected(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta, out Vector6 ret);
			return ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 Lerp(Vector6 a, Vector6 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Vector6(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.a + (b.a - a.a) * t, a.b + (b.b - a.b) * t, a.c + (b.c - a.c) * t);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 LerpUnclamped(Vector6 a, Vector6 b, float t)
		{
			return new Vector6(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.a + (b.a - a.a) * t, a.b + (b.b - a.b) * t, a.c + (b.c - a.c) * t);
		}

		public static Vector6 MoveTowards(Vector6 current, Vector6 target, float maxDistanceDelta)
		{
			float num = target.x - current.x;
			float num2 = target.y - current.y;
			float num3 = target.z - current.z;
			float num4 = target.x - current.x;
			float num5 = target.y - current.y;
			float num6 = target.z - current.z;
			float num7 = num * num + num2 * num2 + num3 * num3 + num4 * num4 + num5 * num5 + num6 * num6;
			if (num7 == 0f || (maxDistanceDelta >= 0f && num7 <= maxDistanceDelta * maxDistanceDelta))
			{
				return target;
			}

			float num8 = (float)Math.Sqrt(num7);
			return new Vector6(current.x + num / num8 * maxDistanceDelta, current.y + num2 / num8 * maxDistanceDelta, current.z + num3 / num8 * maxDistanceDelta, current.x + num4 / num8 * maxDistanceDelta, current.y + num5 / num8 * maxDistanceDelta, current.z + num6 / num8 * maxDistanceDelta);
		}

		[ExcludeFromDocs]
		public static Vector6 SmoothDamp(Vector6 current, Vector6 target, ref Vector6 currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static Vector6 SmoothDamp(Vector6 current, Vector6 target, ref Vector6 currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static Vector6 SmoothDamp(Vector6 current, Vector6 target, ref Vector6 currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			float num;
			float num2;
			float num3;
			float numa;
			float numb;
			float numc;
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num4 = 2f / smoothTime;
			float num5 = num4 * deltaTime;
			float num6 = 1f / (1f + num5 + 0.48f * num5 * num5 + 0.235f * num5 * num5 * num5);
			float num7 = current.x - target.x;
			float num8 = current.y - target.y;
			float num9 = current.z - target.z;
			float numta = current.a - target.a;
			float numtb = current.b - target.b;
			float numtc = current.c - target.c;
			Vector6 vector = target;
			float num10 = maxSpeed * smoothTime;
			float num11 = num10 * num10;
			float num12 = num7 * num7 + num8 * num8 + num9 * num9 + numta * numta + numtb * numtb + numtc * numtc;
			if (num12 > num11)
			{
				float num13 = (float)Math.Sqrt(num12);
				num7 = num7 / num13 * num10;
				num8 = num8 / num13 * num10;
				num9 = num9 / num13 * num10;
			}

			target.x = current.x - num7;
			target.y = current.y - num8;
			target.z = current.z - num9;
			target.a = current.a - numta;
			target.b = current.b - numtb;
			target.c = current.c - numtc;
			float num14 = (currentVelocity.x + num4 * num7) * deltaTime;
			float num15 = (currentVelocity.y + num4 * num8) * deltaTime;
			float num16 = (currentVelocity.z + num4 * num9) * deltaTime;
			float num14a = (currentVelocity.a + num4 * numta) * deltaTime;
			float num15b = (currentVelocity.b + num4 * numtb) * deltaTime;
			float num16c = (currentVelocity.c + num4 * numtc) * deltaTime;
			currentVelocity.x = (currentVelocity.x - num4 * num14) * num6;
			currentVelocity.y = (currentVelocity.y - num4 * num15) * num6;
			currentVelocity.z = (currentVelocity.z - num4 * num16) * num6;
			currentVelocity.a = (currentVelocity.a - num4 * num14a) * num6;
			currentVelocity.b = (currentVelocity.b - num4 * num15b) * num6;
			currentVelocity.c = (currentVelocity.c - num4 * num16c) * num6;
			num = target.x + (num7 + num14) * num6;
			num2 = target.y + (num8 + num15) * num6;
			num3 = target.z + (num9 + num16) * num6;
			numa = target.a + (numta + num14a) * num6;
			numb = target.b + (numtb + num15b) * num6;
			numc = target.c + (numtc + num16c) * num6;
			float num17 = vector.x - current.x;
			float num18 = vector.y - current.y;
			float num19 = vector.z - current.z;
			float num17a = vector.a - current.a;
			float num18b = vector.b - current.b;
			float num19c = vector.c - current.c;
			float num20 = num - vector.x;
			float num21 = num2 - vector.y;
			float num22 = num3 - vector.z;
			float num20a = numa - vector.a;
			float num21b = numb - vector.b;
			float num22c = numc - vector.c;
			if (num17 * num20 + num18 * num21 + num19 * num22 + num17a * num20a + num18b * num21b + num19c * num22c > 0f)
			{
				num = vector.x;
				num2 = vector.y;
				num3 = vector.z;
				numa = vector.a;
				numb = vector.b;
				numc = vector.c;
				currentVelocity.x = (num - vector.x) / deltaTime;
				currentVelocity.y = (num2 - vector.y) / deltaTime;
				currentVelocity.z = (num3 - vector.z) / deltaTime;
				currentVelocity.a = (numa - vector.a) / deltaTime;
				currentVelocity.b = (numb - vector.b) / deltaTime;
				currentVelocity.c = (numc - vector.c) / deltaTime;
			}

			return new Vector6(num, num2, num3, numa, numb, numc);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector6(float x, float y, float z, float a, float b, float c)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.a = a;
			this.b = b;
			this.c = c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector6(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			a = 0f;
			b = 0f;
			c = 0f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector6(float x, float y)
		{
			this.x = x;
			this.y = y;
			z = 0f;
			a = 0f;
			b = 0f;
			c = 0f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(float newX, float newY, float newZ, float newA, float newB, float newC)
		{
			x = newX;
			y = newY;
			z = newZ;
			a = newA;
			b = newB;
			c = newC;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 Scale(Vector6 a, Vector6 b)
		{
			return new Vector6(a.x * b.x, a.y * b.y, a.z * b.z, a.a * b.a, a.b * b.b, a.c * b.c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector6 scale)
		{
			x *= scale.x;
			y *= scale.y;
			z *= scale.z;
			a *= scale.a;
			b *= scale.b;
			c *= scale.c;
		}

		//
		public static Vector6 Cross(Vector6 lhs, Vector6 rhs)
		{
			return new Vector6(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector6))
			{
				return false;
			}

			return Equals((Vector6)other);
		}

		public bool Equals(Vector6 other)
		{
			return x == other.x && y == other.y && z == other.z;
		}

		public static Vector6 Reflect(Vector6 inDirection, Vector6 inNormal)
		{
			float num = -2f * Dot(inNormal, inDirection);
			return new Vector6(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z, num * inNormal.a + inDirection.a, num * inNormal.b + inDirection.b, num * inNormal.c + inDirection.c);
		}

		public static Vector6 Normalize(Vector6 value)
		{
			float num = Magnitude(value);
			if (num > 1E-05f)
			{
				return value / num;
			}

			return zero;
		}

		public void Normalize()
		{
			float num = Magnitude(this);
			if (num > 1E-05f)
			{
				this /= num;
			}
			else
			{
				this = zero;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector6 lhs, Vector6 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.a * rhs.a + lhs.b * rhs.b + lhs.c * rhs.c;
		}

		public static Vector6 Project(Vector6 vector, Vector6 onNormal)
		{
			float num = Dot(onNormal, onNormal);
			if (num < Mathf.Epsilon)
			{
				return zero;
			}

			float num2 = Dot(vector, onNormal);
			return new Vector6(onNormal.x * num2 / num, onNormal.y * num2 / num, onNormal.z * num2 / num);
		}

		public static Vector6 ProjectOnPlane(Vector6 vector, Vector6 planeNormal)
		{
			float num = Dot(planeNormal, planeNormal);
			if (num < Mathf.Epsilon)
			{
				return vector;
			}

			float num2 = Dot(vector, planeNormal);
			return new Vector6(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num, vector.a - planeNormal.a * num2 / num, vector.b - planeNormal.b * num2 / num, vector.c - planeNormal.c * num2 / num);
		}

		public static float Angle(Vector6 from, Vector6 to)
		{
			float num = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
			if (num < 1E-15f)
			{
				return 0f;
			}

			float num2 = Mathf.Clamp(Dot(from, to) / num, -1f, 1f);
			return (float)Math.Acos(num2) * 57.29578f;
		}

		//
		public static float SignedAngle(Vector6 from, Vector6 to, Vector6 axis)
		{
			float num = Angle(from, to);
			float num2 = from.y * to.z - from.z * to.y;
			float num3 = from.z * to.x - from.x * to.z;
			float num4 = from.x * to.y - from.y * to.x;
			float num5 = Mathf.Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
			return num * num5;
		}

		public static float Distance(Vector6 a, Vector6 b)
		{
			float num = a.x - b.x;
			float num2 = a.y - b.y;
			float num3 = a.z - b.z;
			return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		}

		public static Vector6 ClampMagnitude(Vector6 vector, float maxLength)
		{
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude > maxLength * maxLength)
			{
				float num = (float)Math.Sqrt(sqrMagnitude);
				float num2 = vector.x / num;
				float num3 = vector.y / num;
				float num4 = vector.z / num;
				float num5 = vector.a / num;
				float num6 = vector.b / num;
				float num7 = vector.c / num;
				return new Vector6(num2 * maxLength, num3 * maxLength, num4 * maxLength, num5 * maxLength, num6 * maxLength, num7 * maxLength);
			}

			return vector;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Magnitude(Vector6 vector)
		{
			return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.a * vector.a + vector.b * vector.b + +vector.c * vector.c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrMagnitude(Vector6 vector)
		{
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.a * vector.a + vector.b * vector.b + vector.c * vector.c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 Min(Vector6 lhs, Vector6 rhs)
		{
			return new Vector6(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z), Mathf.Min(lhs.a, rhs.a), Mathf.Min(lhs.b, rhs.b), Mathf.Min(lhs.c, rhs.c));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 Max(Vector6 lhs, Vector6 rhs)
		{
			return new Vector6(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z), Mathf.Max(lhs.a, rhs.a), Mathf.Max(lhs.b, rhs.b), Mathf.Max(lhs.c, rhs.c));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator +(Vector6 a, Vector6 b)
		{
			return new Vector6(a.x + b.x, a.y + b.y, a.z + b.z, a.c + b.c, a.c + b.c, a.c + b.c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator -(Vector6 a, Vector6 b)
		{
			return new Vector6(a.x - b.x, a.y - b.y, a.z - b.z, a.a - b.a, a.b - b.b, a.c - b.c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator -(Vector6 a)
		{
			return new Vector6(0f - a.x, 0f - a.y, 0f - a.z, 0f - a.a, 0f - a.b, 0f - a.c);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator *(Vector6 a, float d)
		{
			return new Vector6(a.x * d, a.y * d, a.z * d, a.a * d, a.b * d, a.c * d);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator *(float d, Vector6 a)
		{
			return new Vector6(a.x * d, a.y * d, a.z * d, a.a * d, a.b * d, a.c * d);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector6 operator /(Vector6 a, float d)
		{
			return new Vector6(a.x / d, a.y / d, a.z / d, a.a / d, a.b / d, a.c / d);
		}

		public static bool operator ==(Vector6 lhs, Vector6 rhs)
		{
			float num = lhs.x - rhs.x;
			float num2 = lhs.y - rhs.y;
			float num3 = lhs.z - rhs.z;
			float num4 = lhs.a - rhs.a;
			float num5 = lhs.b - rhs.b;
			float num6 = lhs.c - rhs.c;
			float num7 = num * num + num2 * num2 + num3 * num3 + num4 * num4 + num5 * num5 + num6 * num6;
			return num7 < 9.99999944E-11f;
		}

		public static bool operator !=(Vector6 lhs, Vector6 rhs)
		{
			return !(lhs == rhs);
		}

		public override string ToString()
		{
			return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
		}

		public string ToString(string format)
		{
			return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (string.IsNullOrEmpty(format))
			{
				format = "F1";
			}

			return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), z.ToString(format, formatProvider), a.ToString(format, formatProvider), b.ToString(format, formatProvider), c.ToString(format, formatProvider));
		}

		[Obsolete("Use Vector6.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
		public static float AngleBetween(Vector6 from, Vector6 to)
		{
			return (float)Math.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f));
		}

		[Obsolete("Use Vector6.ProjectOnPlane instead.")]
		public static Vector6 Exclude(Vector6 excludeThis, Vector6 fromThat)
		{
			return ProjectOnPlane(fromThat, excludeThis);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Slerp_Injected(ref Vector6 a, ref Vector6 b, float t, out Vector6 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SlerpUnclamped_Injected(ref Vector6 a, ref Vector6 b, float t, out Vector6 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RotateTowards_Injected(ref Vector6 current, ref Vector6 target, float maxRadiansDelta, float maxMagnitudeDelta, out Vector6 ret);
	}
}
