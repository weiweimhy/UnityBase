using UnityEngine;
using System.Text;

namespace BaseFramework
{
    public static class TransformExtension
    {
        public static Transform Reset(this Transform self)
        {
            self.localPosition = Vector3.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
            return self;
        }

        #region local position
        public static Vector3 GetLocalPosition(this Transform self)
        {
            return self.localPosition;
        }

        public static Transform LocalPosition(this Transform self, Vector3 localPosition)
        {
            self.localPosition = localPosition;
            return self;
        }

        public static Transform LocalPositionX(this Transform self, float x)
        {
            self.localPosition = self.localPosition.NewX(x);
            return self;
        }

        public static Transform LocalPositionY(this Transform self, float y)
        {
            self.localPosition = self.localPosition.NewY(y);
            return self;
        }

        public static Transform LocalPositionZ(this Transform self, float z)
        {
            self.localPosition = self.localPosition.NewZ(z);
            return self;
        }

        public static Transform LocalPositionReset(this Transform self)
        {
            self.localPosition = Vector3.zero;
            return self;
        }
        #endregion

        #region local rotation

        public static Quaternion GetLocalRotaiton(this Transform self)
        {
            return self.localRotation;
        }

        public static Transform LocalRotation(this Transform self, Quaternion localRotation)
        {
            self.localRotation = localRotation;
            return self;
        }

        public static Transform LocalRotationReset(this Transform self)
        {
            self.localRotation = Quaternion.identity;
            return self;
        }
        #endregion

        #region local scale

        public static Vector3 GetLocalScale(this Transform self)
        {
            return self.localScale;
        }

        public static Transform LocalScale(this Transform self, Vector3 scale)
        {
            self.localScale = scale;
            return self;
        }

        public static Transform LocalScale(this Transform self, float value)
        {
            self.localScale = Vector3.one * value;
            return self;
        }

        public static Transform LocalScaleX(this Transform self, float x)
        {
            self.localScale = self.localScale.NewX(x);
            return self;
        }

        public static Transform LocalScaleY(this Transform self, float y)
        {
            self.localScale = self.localScale.NewY(y);
            return self;
        }

        public static Transform LocalScaleZ(this Transform self, float z)
        {
            self.localScale = self.localScale.NewZ(z);
            return self;
        }

        public static Transform LocalScaleReset(this Transform self)
        {
            self.localScale = Vector3.one;
            return self;
        }
        #endregion

        #region position
        public static Vector3 GetPosition(this Transform self)
        {
            return self.position;
        }

        public static Transform Position(this Transform self, Vector3 position)
        {
            self.position = position;
            return self;
        }

        public static Transform PositionX(this Transform self, float x)
        {
            self.position = self.position.NewX(x);
            return self;
        }

        public static Transform PositionY(this Transform self, float y)
        {
            self.position = self.position.NewY(y);
            return self;
        }

        public static Transform PositionZ(this Transform self, float z)
        {
            self.position = self.position.NewZ(z);
            return self;
        }

        public static Transform PositionReset(this Transform self)
        {
            self.position = Vector3.zero;
            return self;
        }
        #endregion

        #region rotation

        public static Quaternion Rotation(this Transform self)
        {
            return self.rotation;
        }

        public static Transform Rotation(this Transform self, Quaternion rotation)
        {
            self.rotation = rotation;
            return self;
        }

        public static Transform RotationReset(this Transform self)
        {
            self.rotation = Quaternion.identity;
            return self;
        }
        #endregion

        #region lossy scale
        public static Vector3 GetLossyScale(this Transform self)
        {
            return self.lossyScale;
        }
        #endregion

        #region child

        public static Transform DestroyAllChildren(this Transform self, float defaultDelay)
        {
            for (int i = 0; i < self.childCount; ++i)
            {
                self.GetChild(i).Destroy(defaultDelay);
            }

            return self;
        }


        public static void SetActiveAllChildren(this Transform self, bool active, bool recursive = false)
        {
            for (int i = 0; i < self.childCount; ++i)
            {
                Transform child = self.GetChild(i);
                child.gameObject.SetActive(active);
                if (recursive)
                {
                    child.SetActiveAllChildren(active, recursive);
                }
            }
        }

        #endregion

        #region find

        public static Transform FindChild(this Transform self, System.Predicate<Transform> predicate)
        {
            for (int i = 0; i < self.childCount; ++i)
            {
                Transform child = self.GetChild(i);
                if (predicate(child))
                {
                    return child;
                }
            }
            return null;
        }

        public static T FindChild<T>(this Transform self, System.Predicate<T> predicate) where T : Component
        {
            for (int i = 0; i < self.childCount; ++i)
            {
                T child = self.GetChild(i).GetComponent<T>();
                if (predicate(child))
                {
                    return child;
                }
            }
            return null;
        }

        public static Transform FindFirstActiveChild(this Transform self)
        {
            return self.FindChild((child) => child.gameObject.activeSelf);
        }

        public static Transform FindFirstInactiveChild(this Transform self)
        {
            return self.FindChild((child) => !child.gameObject.activeSelf);
        }

        public static T FindFirstActiveChild<T>(this Transform self) where T : Component
        {
            return self.FindChild<T>((child) => child.gameObject.activeSelf);
        }

        public static T FindFirstInactiveChild<T>(this Transform self) where T : Component
        {
            return self.FindChild<T>((child) => !child.gameObject.activeSelf);
        }

        #endregion

        public static Transform CopyInfo(this Transform self, Transform targetTransform)
        {
            self.SetParent(targetTransform.parent);
            self.localPosition = targetTransform.localPosition;
            self.localRotation = targetTransform.localRotation;
            self.localScale = targetTransform.localScale;

            return self;
        }

        public static string GetPath(this Transform transform)
        {
            StringBuilder sb = new StringBuilder(transform.name);
            for (Transform temp = transform.parent;
                temp != null;
                sb.Insert(0, temp.name + "/"), temp = temp.parent);
            return sb.ToString();
        }
    }
}
