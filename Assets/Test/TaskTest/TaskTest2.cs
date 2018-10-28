using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseFramework
{
    public class TaskTest2 : MonoBehaviour {

        // Use this for initialization
        void Start() {
            this.Delay(2,() => {
                Log.I(this, "延迟执行");
            });
    
        }

        // Update is called once per frame
        void Update() {

        }
    }
}