using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseFramework.Test
{
    public class SwitchButton : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            TaskHelper.Create<CoroutineTask>()
                .Name("task inner")
                .MonoBehaviour(this)
                .Delay(10)
                .Do(() => { Log.I(this, "task inner do"); })
                .Execute();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        private void OnDisable()
        {
            Log.I(this,"OnDisable");
        }
    }
}