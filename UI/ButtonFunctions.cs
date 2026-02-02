using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archmage.UI {
    public class ButtonFunctions : MonoBehaviour {

        [SerializeField] private GameObject selectPanel;
        [SerializeField] private GameObject optionsMenu;
        public void StartGame() {
            // Load the first level (Assumes "Level1" is the first level)
            if (GameManager.instance != null && GameManager.instance.levels.Length > 0) {
                // Use GamaeManager's level array to get the first level
                SceneManager.LoadScene(GameManager.instance.levels[0]);
            } else {
                // Fall back load a specific scene directly if GameManager doesn't handle levels
                SceneManager.LoadScene("Level0");
            }
        }

        public void Options()
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.OpenOptionsMenu();
            }
        }

        public void CloseOptionsMenu()
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.CloseOptionsMenu();
            }
        }


        public void OptionsMainMenu()
        {
            if (selectPanel != null) 
                selectPanel.SetActive(false);
            if (optionsMenu != null) 
                optionsMenu.SetActive(true);
        }

        public void CloseOptionsMenuMainMenu()
        {
            if (selectPanel != null) 
                selectPanel.SetActive(true);
            if (optionsMenu != null) 
                optionsMenu.SetActive(false);
        }

        public void Resume() {
            // Unpause the game via resume button
            GameManager.instance.StateUnpause();
        }

        public void Restart() {
            if (GameManager.instance.gameOver == true)
                GameManager.instance.gameOver = false;

            // Restart the level via restart button
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // Unpause the game
            GameManager.instance.StateUnpause();
        }

        public void Respawn() {

            GameManager.instance.playerController.PlayerCheckpoint.RespawnAtCheckpoint();
        }

        public void OnApplicationQuit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
            QuitApp quitApp = new QuitApp();
            quitApp.QuitAndClose();
        #else
            Application.Quit();
        #endif
        }

        // Load Next Level
        public void LoadNextLevel() {
            if (GameManager.instance != null) {
                Debug.Log("ButtonFunctions trigger LoadNextLevel.");
                GameManager.instance.LoadNextLevel();  // Load the next level
            } else {
                Debug.LogError("GameManager.instance is null. LoadNextLevel failed.");
            }
        }

        // Return to Main Menu
        public void LoadMainMenu() {
            if (GameManager.instance != null) {
                GameManager.instance.LoadMainMenu();  // Load the main menu
            }
        }
        public void GameEnd()
        {
            SceneManager.LoadScene(0);
        }
        public void LoadCredits()
        {
            SceneManager.LoadScene(9);
        }
    }
}
