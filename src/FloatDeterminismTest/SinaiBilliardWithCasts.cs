using System;

namespace FloatDeterminismTest {
	/// <summary>
	/// https://en.wikipedia.org/wiki/Dynamical_billiards#Sinai's_billiards
	/// </summary>
	internal class SinaiBilliardWithCasts {

		public static Vector3f Simulate(Ray3f ray, int steps) {
			ray = new Ray3f(ray.Origin, normalizeWithCasts(ray.Direction));
			Aabb box = new Aabb(new Vector3f(-2f, -2f, -2f), new Vector3f(2f, 2f, 2f));

			bool reflectedFromSphere = false;
			Vector3f isectPoint = ray.Origin;

			for (int i = 0; i < steps; i++) {
				bool intersected = intersectAabbWithCasts(ray, box, out float _, out int __, out float tBox, out int normalIndex);
				if (intersected == false) {
					throw new Exception("No intersection!");
				}

				if (reflectedFromSphere == false
						&& intersectUnitSphereWithCasts(ray, out float tSphere, out _)) {
					if (tSphere > 0f && tSphere < tBox) {
						// We intersected the sphere.
						isectPoint = normalizeWithCasts(getPointWithCasts(ray, tSphere));
						ray = new Ray3f(isectPoint,
							normalizeWithCasts(reflectWithCasts(ray.Direction, isectPoint)));
						reflectedFromSphere = true;
						continue;
					}
				}

				// We intersected the cube.
				isectPoint = getPointWithCasts(ray, tBox);
				ray = new Ray3f(isectPoint,
					normalizeWithCasts(reflectWithCasts(ray.Direction, SinaiBilliard.NORMALS[normalIndex])));
				reflectedFromSphere = false;
			}

			return isectPoint;
		}

		private static Vector3f getPointWithCasts(Ray3f ray, float distance) {
			return new Vector3f(
				ray.Origin.X + (float)(distance * ray.Direction.X),
				ray.Origin.Y + (float)(distance * ray.Direction.Y),
				ray.Origin.Z + (float)(distance * ray.Direction.Z));
		}

		private static Vector3f normalizeWithCasts(Vector3f v) {
			float lengthSqr = (float)dotWithCasts(v, v);
			if (lengthSqr < SinaiBilliard.EPSILON) {
				throw new Exception("Normalizing zero vector.");
			}

			float length = (float)Math.Sqrt(lengthSqr);
			return new Vector3f((float)(v.X / length), (float)(v.Y / length), (float)(v.Z / length));
		}

		private static Vector3f reflectWithCasts(Vector3f v, Vector3f normal) {
			float twoDot = (float)(2f * dotWithCasts(v, normal));
			return new Vector3f(
				(float)(v.X - (float)(twoDot * normal.X)),
				(float)(v.Y - (float)(twoDot * normal.Y)),
				(float)(v.Z - (float)(twoDot * normal.Z)));
		}

		private static float dotWithCasts(Vector3f lhs, Vector3f rhs) {
			return (float)(lhs.X * rhs.X) + (float)(lhs.Y * rhs.Y) + (float)(lhs.Z * rhs.Z);
		}

		private static bool intersectUnitCubeWithCasts(Ray3f ray, out float tMin, out int minNormalIndex,
				out float tMax, out int maxNormalIndex) {
			tMin = float.NegativeInfinity;
			tMax = float.PositiveInfinity;
			minNormalIndex = -1;
			maxNormalIndex = -1;

			// X axis
			if (Math.Abs(ray.Direction.X) < SinaiBilliard.EPSILON) {
				if (ray.Origin.X < 0f || ray.Origin.X > 1f) {
					return false;
				}
			} else {
				float mul = (float)(1f / ray.Direction.X);
				float t1 = (float)(-ray.Origin.X * mul);
				float t2 = (float)(t1 + mul);

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
			if (Math.Abs(ray.Direction.Y) < SinaiBilliard.EPSILON) {
				if (ray.Origin.Y < 0f || ray.Origin.Y > 1f) {
					return false;
				}
			} else {
				float mul = (float)(1f / ray.Direction.Y);
				float t1 = (float)(-ray.Origin.Y * mul);
				float t2 = (float)(t1 + mul);

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
			if (Math.Abs(ray.Direction.Z) < SinaiBilliard.EPSILON) {
				if (ray.Origin.Z < 0f || ray.Origin.Z > 1f) {
					return false;
				}
			} else {
				float mul = (float)(1f / ray.Direction.Z);
				float t1 = (float)(-ray.Origin.Z * mul);
				float t2 = (float)(t1 + mul);

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

		private static bool intersectAabbWithCasts(Ray3f ray, Aabb aabb, out float tMin, out int minNormalIndex,
				out float tMax, out int maxNormalIndex) {
			Vector3f aabbSize = aabb.GetSize();
			Vector3f newDirection = new Vector3f((float)(ray.Direction.X / aabbSize.X), (float)(ray.Direction.Y / aabbSize.Y),
				(float)(ray.Direction.Z / aabbSize.Z));
			Ray3f newRay = new Ray3f(new Vector3f(
					(float)(ray.Origin.X - aabb.Min.X) / aabbSize.X,
					(float)(ray.Origin.Y - aabb.Min.Y) / aabbSize.Y,
					(float)(ray.Origin.Z - aabb.Min.Z) / aabbSize.Z
				), normalizeWithCasts(newDirection));
			if (intersectUnitCubeWithCasts(newRay, out tMin, out minNormalIndex, out tMax, out maxNormalIndex)
					== false) {
				return false;
			}

			float scale = (float)(1f / (float)Math.Sqrt(dotWithCasts(newDirection, newDirection)));
			tMin *= scale;
			tMax *= scale;
			return true;
		}

		private static bool intersectUnitSphereWithCasts(Ray3f ray, out float tMin, out float tMax) {
			float sd = dotWithCasts(ray.Origin, ray.Direction);
			float ss = dotWithCasts(ray.Origin, ray.Origin);

			float discrOver4 = (float)((float)((float)(sd * sd) - ss) + 1f);
			if (discrOver4 < 0.0f) {
				tMin = float.NaN;
				tMax = float.NaN;
				return false;
			}

			float discrOver4Sqrt = (float)Math.Sqrt(discrOver4);
			tMin = (float)(-sd - discrOver4Sqrt);
			tMax = (float)(-sd + discrOver4Sqrt);
			return true;
		}

	}

}
