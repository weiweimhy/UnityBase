using UnityEngine;

namespace BaseFramework
{
    public static class Vector3Extension
    {
        // 缓存一些变量
        private static Vector2 cacheV2;
        private static Vector3 cacheV3;

        public static Vector2 ToV2(this Vector3 self)
        {
            return self;
        }

        public static Vector2 ToV2XZ(this Vector3 self)
        {
            cacheV2.x = self.x;
            cacheV2.y = self.z;
            return cacheV2;
        }

        public static Vector2 XZV2(this Vector3 self)
        {
            return self.ToV2XZ();
        }

        public static Vector2 ToV2YZ(this Vector3 self)
        {
            cacheV2.x = self.y;
            cacheV2.y = self.z;
            return cacheV2;
        }

        public static Vector2 YZV2(this Vector3 self)
        {
            return self.ToV2YZ();
        }

        public static Vector3 NewX(this Vector3 self, float x)
        {
            cacheV3 = self;
            cacheV3.x = x;
            return cacheV3;
        }

        public static Vector3 NewY(this Vector3 self, float y)
        {
            cacheV3 = self;
            cacheV3.y = y;
            return cacheV3;
        }

        public static Vector3 NewZ(this Vector3 self, float z)
        {
            cacheV3 = self;
            cacheV3.z = z;
            return cacheV3;
        }

        public static Vector3 NewXY(this Vector3 self, float x, float y)
        {
            return self.NewX(x).NewY(y);
        }

        public static Vector3 NewXZ(this Vector3 self, float x, float z)
        {
            return self.NewX(x).NewZ(z);
        }

        public static Vector3 NewYZ(this Vector3 self, float y, float z)
        {
            return self.NewY(y).NewZ(z);
        }

        public static Vector3 OffsetX(this Vector3 self, float x)
        {
            return self.NewX(self.x + x);
        }

        public static Vector3 OffsetY(this Vector3 self, float y)
        {
            return self.NewY(self.y + y);
        }

        public static Vector3 OffsetZ(this Vector3 self, float z)
        {
            return self.NewZ(self.z + z);
        }

        public static Vector3 OffsetXY(this Vector3 self, float x, float y)
        {
            return self.NewXY(self.x + x, self.y + y);
        }

        public static Vector3 OffsetXZ(this Vector3 self, float x, float z)
        {
            return self.NewXZ(self.x + x, self.z + z);
        }

        public static Vector3 OffsetYZ(this Vector3 self, float y, float z)
        {
            return self.NewYZ(self.y + y, self.z + z);
        }

        public static Vector3 ScaleX(this Vector3 self, float x)
        {
            return self.NewX(self.x * x);
        }

        public static Vector3 ScaleY(this Vector3 self, float y)
        {
            return self.NewY(self.y * y);
        }

        public static Vector3 ScaleZ(this Vector3 self, float z)
        {
            return self.NewZ(self.z * z);
        }

        public static Vector3 ScaleXY(this Vector3 self, float x, float y)
        {
            return self.NewXY(self.x * x, self.y * y);
        }

        public static Vector3 ScaleXZ(this Vector3 self, float x, float z)
        {
            return self.NewXZ(self.x * x, self.z * z);
        }

        public static Vector3 ScaleYZ(this Vector3 self, float y, float z)
        {
            return self.NewYZ(self.y * y, self.z * z);
        }

        /// <summary>
        /// x,y,z的值是否相同
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsUniformScale(this Vector3 self)
        {
            return Mathf.Approximately(self.x, self.y) && Mathf.Approximately(self.x, self.z);
        }

        public static float AngleOnXZ(this Vector3 self)
        {
            return Vector3.right.AngleTo(self.NewY(0), Vector3.down);
        }

        public static float AngleTo(this Vector3 from, Vector3 to, Vector3 axis)
        {
            float angle = Vector3.Angle(from, to);
            float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(from, to)));
            return angle * sign;
        }

        public static Vector3 RotateBy(this Vector3 self, float angle, Vector3 axis)
        {
            return Quaternion.AngleAxis(angle, axis) * self;
        }

        public static Vector3 Clamp(this Vector3 self, Bounds bounds)
        {
            return self.Clamp(bounds.min, bounds.max);
        }

        public static Vector3 Clamp(this Vector3 self, Vector3 min, Vector3 max)
        {
            cacheV3.x = Mathf.Clamp(self.x, min.x, max.x);
            cacheV3.y = Mathf.Clamp(self.y, min.y, max.y);
            cacheV3.z = Mathf.Clamp(self.z, min.z, max.z);
            return cacheV3;
        }

        /// <summary>
        /// 在中心为center，半径为radius的球面上产生随机点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector3 RandomPositionOnSphere(this Vector3 center, float radius)
        {
            return center + Random.onUnitSphere * radius;
        }

        public static Vector3 RandomPositionInSphere(this Vector3 center, float maxRadius, float minRadius = 0)
        {
            return center + Random.onUnitSphere * Random.Range(minRadius, maxRadius);
        }

        public static Transform GetNearest(this Vector3 center, float radius, int layerMask)
        {
            Transform nearest = null;
            Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask);
            float minDistance = float.MaxValue;
            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(center, collider.transform.position);
                if (distance < minDistance)
                {
                    nearest = collider.transform;
                    minDistance = distance;
                }
            }
            return nearest;
        }

        public static Transform GetNearestNonAlloc(this Vector3 center, float radius, Collider[] colliders, int layerMask)
        {
            Transform nearest = null;
            int count = Physics.OverlapSphereNonAlloc(center, radius, colliders, layerMask);
            float minDistance = float.MaxValue;
            for (int i = 0; i < count; ++i)
            {
                float distance = Vector3.Distance(center, colliders[i].transform.position);
                if (distance < minDistance)
                {
                    nearest = colliders[i].transform;
                    minDistance = distance;
                }
            }
            return nearest;
        }

        public static Transform RayCast2D(this Vector3 self, Vector2 direction,
                                          float maxDistance = Mathf.Infinity,
                                          int layerMask = Physics2D.DefaultRaycastLayers)
        {
            RaycastHit2D hit = Physics2D.Raycast(self, direction, maxDistance, layerMask);
            return hit.transform;
        }

        public static T RayCast2D<T>(this Vector3 self, Vector2 direction,
                                     float maxDistance = Mathf.Infinity,
                                     int layerMask = Physics.DefaultRaycastLayers) where T : Component
        {
            Transform target = self.RayCast2D(direction, maxDistance, layerMask);
            return (target != null) ? target.GetComponent<T>() : null;
        }

        public static Transform RayCast3D(this Vector3 self, Vector3 direction,
                                          float maxDistance = Mathf.Infinity,
                                          int layerMask = Physics.DefaultRaycastLayers)
        {
            RaycastHit hit;
            if (Physics.Raycast(self, direction, out hit, maxDistance, layerMask))
            {
                return hit.transform;
            }
            return null;
        }

        public static T RayCast3D<T>(this Vector3 self, Vector3 direction,
                                        float maxDistance = Mathf.Infinity,
                                        int layerMask = Physics.DefaultRaycastLayers) where T : Component
        {
            Transform target = self.RayCast3D(direction, maxDistance, layerMask);
            return (target != null) ? target.GetComponent<T>() : null;
        }
    }
}