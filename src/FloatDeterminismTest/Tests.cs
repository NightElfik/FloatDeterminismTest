// MAFI: NOFORMAT
using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace FloatDeterminismTest {
	[TestFixture]
	public class Tests {
		
		private static readonly Vector3f[] ANSWERS_WITHOUT_CASTS = new [] {
#if DEBUG
			new Vector3f(F2U.Convert(0x40000001), F2U.Convert(0x3E668C38), F2U.Convert(0x3FF816CA)),
			new Vector3f(F2U.Convert(0xBF8F42AF), F2U.Convert(0xBE6B6C76), F2U.Convert(0x40000000)),
			new Vector3f(F2U.Convert(0x3EF60FFC), F2U.Convert(0xC0000000), F2U.Convert(0x3FEC1648)),
			new Vector3f(F2U.Convert(0xC0000000), F2U.Convert(0xBEA3C421), F2U.Convert(0xBFEB6CEC)),
			new Vector3f(F2U.Convert(0x3FCD7621), F2U.Convert(0xBFF871CB), F2U.Convert(0x40000000)),
			new Vector3f(F2U.Convert(0xC0000000), F2U.Convert(0x3F4718B3), F2U.Convert(0x3FEAE7E3)),
#else
			new Vector3f(F2U.Convert(0x3FFFFFFF), F2U.Convert(0x3E669153), F2U.Convert(0x3FF816B6)),
			new Vector3f(F2U.Convert(0xBF9F599D), F2U.Convert(0x3FFFFFFE), F2U.Convert(0x3F606280)),
			new Vector3f(F2U.Convert(0x3D87AEF9), F2U.Convert(0xBF1A0D36), F2U.Convert(0x3FFFFFFF)),
			new Vector3f(F2U.Convert(0x3E9278F7), F2U.Convert(0x40000000), F2U.Convert(0x3FEFB5E4)),
			new Vector3f(F2U.Convert(0xBFB1C90C), F2U.Convert(0xC0000000), F2U.Convert(0xBE5F697B)),
			new Vector3f(F2U.Convert(0x40000000), F2U.Convert(0xBE8505E9), F2U.Convert(0xBEC42A08)),
#endif
		};


		private static readonly Vector3f[] ANSWERS_WITH_CASTS = new [] {
#if DEBUG
			new Vector3f(F2U.Convert(0x40000000), F2U.Convert(0x3E6689D2), F2U.Convert(0x3FF816D9)),
			new Vector3f(F2U.Convert(0x40000000), F2U.Convert(0xBF05A624), F2U.Convert(0x3EECBDB6)),
			new Vector3f(F2U.Convert(0xC0000000), F2U.Convert(0xBF9B43E6), F2U.Convert(0xBEF60590)),
			new Vector3f(F2U.Convert(0x3FD5D2EE), F2U.Convert(0xC0000000), F2U.Convert(0x3E97651B)),
			new Vector3f(F2U.Convert(0xBF3BF460), F2U.Convert(0x3FD7ECDC), F2U.Convert(0x40000000)),
			new Vector3f(F2U.Convert(0xC0000000), F2U.Convert(0x3F4E5CAE), F2U.Convert(0xBF95BFF0)),
#else
			new Vector3f(F2U.Convert(0x3FFFFFFE), F2U.Convert(0x3E6689B0), F2U.Convert(0x3FF816DD)),
			new Vector3f(F2U.Convert(0xBFAEA6E2), F2U.Convert(0xC0000000), F2U.Convert(0x3FCEEFF4)),
			new Vector3f(F2U.Convert(0x3F207DC2), F2U.Convert(0xBF918256), F2U.Convert(0xBFFFFFFE)),
			new Vector3f(F2U.Convert(0x3FEA87EF), F2U.Convert(0x3FFFFFFE), F2U.Convert(0xBEABE9E2)),
			new Vector3f(F2U.Convert(0x3FBE6CA4), F2U.Convert(0xC0000000), F2U.Convert(0x3FD4D2F0)),
			new Vector3f(F2U.Convert(0xBFE22EAC), F2U.Convert(0x40000000), F2U.Convert(0x3F62CA9E)),
#endif
		};
		
		[TestCase(true)]
		public void Test(bool checkAnswers = false) {
			Ray3f ray = new Ray3f(new Vector3f(-0.9f, -0.8f, -0.1f), new Vector3f(0.4f, 0.5f, 0.1f));
			int iterations = 1;

			void writePt(Vector3f pt) {
				Console.WriteLine($"{pt.X,12}, {pt.Y,12}, {pt.Z,12} ("
					+ $"0x{F2U.Convert(pt.X):X}, "
					+ $"0x{F2U.Convert(pt.Y):X}, "
					+ $"0x{F2U.Convert(pt.Z):X})");
			}

			for (int i = 0; i < 6; ++i) {
				iterations *= 10;
				Console.WriteLine(iterations);
				Vector3f pt = SinaiBilliard.Simulate(ray, iterations);
				Console.Write("Without casts: ");
				writePt(pt);
				if (checkAnswers) {
					Assert.AreEqual(pt, ANSWERS_WITHOUT_CASTS[i]);
				}

				Vector3f pt2 = SinaiBilliardWithCasts.Simulate(ray, iterations);
				Console.Write("With casts:    ");
				writePt(pt2);
				if (checkAnswers) {
					Assert.AreEqual(pt2, ANSWERS_WITH_CASTS[i]);
				}

				if (pt.X != pt2.X || pt.Y != pt2.Y || pt.Z != pt2.Z) {
					Console.WriteLine("MISMATCH!!!");
				}
				Console.WriteLine();
			}
		}
	}
	
	internal struct Vector3f {
		public float X, Y, Z;
		public Vector3f(float x, float y, float z) {
			X = x;
			Y = y;
			Z = z;
		}
	}

	internal struct Ray3f {
		public Vector3f Origin, Direction;
		public Ray3f(Vector3f origin, Vector3f direction) {
			Origin = origin;
			Direction = direction;
		}
	}

	internal struct Aabb {
		public Vector3f Min, Max;

		public Aabb(Vector3f min, Vector3f max) {
			Min = min;
			Max = max;
		}

		public Vector3f GetSize() => new Vector3f(Max.X - Min.X, Max.Y - Min.Y, Max.Z - Min.Z);
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct F2U {

		[FieldOffset(0)]
		public float Float;

		[FieldOffset(0)]
		public uint Uint;

		public static uint Convert(float f) {
			return new F2U { Float = f }.Uint;
		}

		public static float Convert(uint u) {
			return new F2U { Uint = u }.Float;
		}

	}

}
