using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework.Test
{
    public class LogTest : MonoBehaviour
    {

        public Log.LogLevel logLevel = Log.LogLevel.VERBOSE;

        public Toggle logUnityStackToggle;
        public Dropdown logLevelSelector;
        public InputField tagInputField;
        public InputField messageInputField;
        public InputField paramInputField;
        public Button logButton;

        private int currentLogLevel = 0;

        private void Awake()
        {
            Log.logLevel = logLevel;

            logLevelSelector.onValueChanged.AddListener((index) => {
                currentLogLevel = index;
            });

            logUnityStackToggle.onValueChanged.AddListener((on) => {
                Log.logUnityStack = on;
            });

            logButton.onClick.AddListener(() => {
                string tag = tagInputField.text;
                string message = messageInputField.text;
                string param = paramInputField.text;
                string[] args = null;
                if (!string.IsNullOrEmpty(param))
                {
                    args = param.Split('|');
                }

                switch (currentLogLevel + Log.LogLevel.DEBUG)
                {
                    case Log.LogLevel.DEBUG:
                        Log.D(tag, message, args);
                        break;
                    case Log.LogLevel.INFO:
                        Log.D(tag, message, args);
                        break;
                    case Log.LogLevel.WARN:
                        Log.W(tag, message, args);
                        break;
                    case Log.LogLevel.ERROR:
                        Log.E(tag, message, args);
                        break;
                }
            });
        }
    }
}
