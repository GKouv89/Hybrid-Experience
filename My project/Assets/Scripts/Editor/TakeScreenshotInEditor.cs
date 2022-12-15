using UnityEngine;
using UnityEditor;

namespace DIMuseumVR.Editor
{
    public class TakeScreenshotInEditor : ScriptableObject
    {
        public static string fileName = "C:/Users/gkara/Documents/DIMuseum/MyScreenshots/";
        public static int startNumber = 1;

        [MenuItem("Screenshot/Take Screenshot of GAME View %&s")]
        static void TakeScreenshot()
        {
            int number = startNumber;
            string name = "" + number;

            // If this is used instead of the manually set folder, then the png file is imported to the project 
            //fileName = Application.dataPath + "/Screenshots/";

            while (System.IO.File.Exists(fileName + name + ".png"))
            {
                number++;
                name = "" + number;
            }

            startNumber = number + 1;

            Debug.Log("Taking screenshot: " + fileName + name + ".png");
            ScreenCapture.CaptureScreenshot(fileName + name + ".png", 2);
        }
    }
}