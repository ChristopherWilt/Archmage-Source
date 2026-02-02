using UnityEngine;
using System.Runtime.InteropServices;

namespace Archmage.UI
{
    public class QuitApp : MonoBehaviour {
        [DllImport("__Internal")]
        private static extern void closewindow();

        public void QuitAndClose() {
            Application.Quit(); // Works in standalone builds

        #if UNITY_WEBGL && !UNITY_EDITOR
            closewindow();  // Calls the JavaScript function in WebGL
        #endif
        }
    }
}