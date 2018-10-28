using UnityEngine;

namespace BaseFramework
{
    public class TaskHelper
    {
        public static T Create<T>() where T : Task<T>
        {
            return SimplePoolHelper.Create<T>();
        }
    }
}