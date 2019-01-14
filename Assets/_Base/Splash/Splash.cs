using UnityEngine;
 
namespace BaseFramework
{
    public abstract class Splash : MonoBehaviour {

        public string targetScene = "Home";
        public float time = 2f;

        // Use this for initialization
        void Start() {
            Invoke("Switch", time);
        }

        public abstract void Switch();
    }
}
