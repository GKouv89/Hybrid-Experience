using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.Utils
{
    public class ConsoleOutput : MonoBehaviour
    {
        public string output = "";
        public string stack = "";

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;

            switch (type)
            {
                case LogType.Log:
                    Debug.Log("Log");
                    break;
                case LogType.Error:
                    Debug.Log("Log");
                    break;
                case LogType.Warning:
                    Debug.Log("Warning");
                    break;
            }
        }
    }
}
