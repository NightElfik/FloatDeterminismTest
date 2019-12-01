using System;

namespace FloatDeterminismTest {
	/// <summary>
	/// https://en.wikipedia.org/wiki/Dynamical_billiards#Sinai's_billiards
	/// </summary>
	internal class SinaiBilliard {
		
		internal const float EPSILON = 1e-6f;

		internal static readonly Vector3f[] NORMALS = {
			new Vector3f(-1, 0, 0),
			new Vector3f(1, 0, 0),
			new Vector3f(0, -1, 0),
			new Vector3f(0, 1, 0),
			new Vector3f(0, 0, -1),
			new Vector3f(0, 0, 1),
		};


		public static Vector3f Simulate(Ray3f ray, int steps) {
			ray = new Ray3f(ray.Origin, normalize(ray.Direction));
			Aabb box = new Aabb(new Vector3f(-2f, -2f, -2f), new Vector3f(2f, 2f, 2f));

			bool reflectedFromSphere = false;
			Vector3f isectPoint = ray.Origin;

			for (int i = 0; i < steps; i++) {
				bool intersected = intersectAabb(ray, box, out float _, out int __, out float tBox, out int normalIndex);
				if (intersected == false) {
					throw new Exception("No intersection!");
				}

				if (reflectedFromSphere == false
						&& intersectUnitSphere(ray, out float tSphere, out _)) {
					if (tSphere > 0f && tSphere < tBox) {
						// We intersected the sphere.
						isectPoint = normalize(getPoint(ray, tSphere));
						ray = new Ray3f(isectPoint,
							normalize(reflect(ray.Direction, isectPoint)));
						reflectedFromSphere = true;
						continue;
					}
				}

				// We intersected the cube.
				isectPoint = getPoint(ray, tBox);
				ray = new Ray3f(isectPoint,
					normalize(reflect(ray.Direction, NORMALS[normalIndex])));
				reflectedFromSphere = false;
			}

			return isectPoint;
		}

		private static Vector3f getPoint(Ray3f ray, float distance) {
			return new Vector3f(
				ray.Origin.X + distance * ray.Direction.X,
				ray.Origin.Y + distance * ray.Direction.Y,
				ray.Origin.Z + distance * ray.Direction.Z);
		}

		private static Vector3f normalize(Vector3f v) {
			float lengthSqr = dot(v, v);
			if (lengthSqr < SinaiBilliard.EPSILON) {
				throw new Exception("Normalizing zero vector.");
			}

			float length = (float)Math.Sqrt(lengthSqr);
			return new Vector3f(v.X / length, v.Y / length, v.Z / length);
		}

		private static Vector3f reflect(Vector3f v, Vector3f normal) {
			float twoDot = 2f * dot(v, normal);
			return new Vector3f(
				v.X - twoDot * normal.X,
				v.Y - twoDot * normal.Y,
				v.Z - twoDot * normal.Z);
		}

		private static float dot(Vector3f lhs, Vector3f rhs) {
			return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
		}

		private static bool intersectUnitCube(Ray3f ray, out float tMin, out int minNormalIndex, out float tMax,
				out int maxNormalIndex) {
			tMin = float.NegativeInfinity;
			tMax = float.PositiveInfinity;
			minNormalIndex = -1;
			maxNormalIndex = -1;

			// X axis
			if (Math.Abs(ray.Direction.X) < EPSILON) {
				if (ray.Origin.X < 0f || ray.Origin.X > 1f) {
					return false;
				}
			} else {
				float mul = 1f / ray.Direction.X;
				float t1 = -ray.Origin.X * mul;
				float t2 = t1 + mul;

				if (mul > 0f) {
					if (t1 > tMin) {
						tMin = t1;
						minNormalIndex = 1;
					}
					if (t2 < tMax) {
						tMax = t2;
						maxNormalIndex = 0;
					}
				} else {
					if (t2 > tMin) {
						tMin = t2;
						minNormalIndex = 0;
					}
					if (t1 < tMax) {
						tMax = t1;
						maxNormalIndex = 1;
					}
				}

				if (tMax < 0f || tMin > tMax) {
					return false;
				}
			}

			// Y axis
			if (Math.Abs(ray.Direction.Y) < EPSILON) {
				if (ray.Origin.Y < 0f || ray.Origin.Y > 1f) {
					return false;
				}
			} else {
				float mul = 1f / ray.Direction.Y;
				float t1 = -ray.Origin.Y * mul;
				float t2 = t1 + mul;

				if (mul > 0f) {
					if (t1 > tMin) {
						tMin = t1;
						minNormalIndex = 3;
					}
					if (t2 < tMax) {
						tMax = t2;
						maxNormalIndex = 2;
					}
				} else {
					if (t2 > tMin) {
						tMin = t2;
						minNormalIndex = 2;
					}
					if (t1 < tMax) {
						tMax = t1;
						maxNormalIndex = 3;
					}
				}

				if (tMax < 0f || tMin > tMax) {
					return false;
				}
			}

			// Z axis
			if (Math.Abs(ray.Direction.Z) < EPSILON) {
				if (ray.Origin.Z < 0f || ray.Origin.Z > 1f) {
					return false;
				}
			} else {
				float mul = 1f / ray.Direction.Z;
				float t1 = -ray.Origin.Z * mul;
				float t2 = t1 + mul;

				if (mul > 0f) {
					if (t1 > tMin) {
						tMin = t1;
						minNormalIndex = 5;
					}
					if (t2 < tMax) {
						tMax = t2;
						maxNormalIndex = 4;
					}
				} else {
					if (t2 > tMin) {
						tMin = t2;
						minNormalIndex = 4;
					}
					if (t1 < tMax) {
						tMax = t1;
						maxNormalIndex = 5;
					}
				}

				if (tMax < 0f || tMin > tMax) {
					return false;
				}
			}

			return true;
		}

		private static bool intersectAabb(Ray3f ray, Aabb aabb, out float tMin, out int minNormalIndex, out float tMax,
				out int maxNormalIndex) {
			Vector3f aabbSize = aabb.GetSize();
			Vector3f newDirection = new Vector3f(ray.Direction.X / aabbSize.X, ray.Direction.Y / aabbSize.Y,
				ray.Direction.Z / aabbSize.Z);
			Ray3f newRay = new Ray3f(new Vector3f(
					(ray.Origin.X - aabb.Min.X) / aabbSize.X,
					(ray.Origin.Y - aabb.Min.Y) / aabbSize.Y,
					(ray.Origin.Z - aabb.Min.Z) / aabbSize.Z
				), normalize(newDirection));
			if (intersectUnitCube(newRay, out tMin, out minNormalIndex, out tMax, out maxNormalIndex)
					== false) {
				return false;
			}

			float scale = 1f / (float)Math.Sqrt(dot(newDirection, newDirection));
			tMin *= scale;
			tMax *= scale;
			return true;
		}

		private static bool intersectUnitSphere(Ray3f ray, out float tMin, out float tMax) {
			float sd = dot(ray.Origin, ray.Direction);
			float ss = dot(ray.Origin, ray.Origin);

			float discrOver4 = sd * sd - ss + 1f;
			if (discrOver4 < 0.0f) {
				tMin = float.NaN;
				tMax = float.NaN;
				return false;
			}

			float discrOver4Sqrt = (float)Math.Sqrt(discrOver4);
			tMin = -sd - discrOver4Sqrt;
			tMax = -sd + discrOver4Sqrt;
			return true;
		}

	}
}
