using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BaseFramework
{
    public static class EventSystemExtension
    {
        public static bool InvokeGracefully(this UnityEvent self)
        {
            if (null != self)
            {
                self.Invoke();
                return true;
            }
            return false;
        }

        public static bool IsPointerOverUIObject(this EventSystem self, GameObject uiObject = null)
        {
            PointerEventData currentPositionEventData = new PointerEventData(self);
            currentPositionEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            self.RaycastAll(currentPositionEventData, results);
            if(uiObject == null)
            {
                return results.Any();
            }
            for (int i = 0; i < results.Count; ++i)
            {
                if (results[i].gameObject == uiObject)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
