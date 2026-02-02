using Archmage.Audio;
using Archmage.Control;
using Archmage.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Archmage.UI {
    public class GameManager : MonoBehaviour {

        // Singleton Setup
        public static GameManager instance;

        // Menus
        [Header("Menus")]
        [SerializeField] GameObject menuActive;     // Tracks the current active menu
        [SerializeField] GameObject menuPause;      // The pause menu
        [SerializeField] GameObject menuOptions;    // The options menu
        [SerializeField] GameObject menuWin;        // The win menu
        [SerializeField] GameObject menuLose;       // The lose menu
        [SerializeField] GameObject menuNextLevel;  // The start menu

        // UI Elements
        [Header("UI Elements")]
        [SerializeField] TMP_Text goalDescription;
        [SerializeField] TMP_Text goalText;                      // Displays the goal count
        [SerializeField] public TMP_Text spellNameText;             // TMP for the equiped spells name
        [SerializeField] public TMP_Text spellDiscriptionText;      // TMP for the equiped spells discription
        [SerializeField] public Image spellImage;                   // Sprite for the equiped spells Icon

        [SerializeField] public TMP_Text healthLevelText;           // TMP for the players health level
        [SerializeField] public TMP_Text manaLevelText;             // TMP for the players mana level

        public GameObject player;                                   // The player object
        public PlayerController playerController;                   // The player script
        public Image playerHPBar;                                   // The player's HP bar
        public Image playerManaBar;                                 // The player's Mana bar
        public GameObject damagePanel;
        public GameObject healPanel;
        public Slider VolumeSlider;
        public Slider VolumeMusicSlider;
        public Slider VolumeSFXSlider;
        public Toggle muteToggle;

        // Enemy UI Elements
        [Header("Enemy UI Elements")]
        [SerializeField] private GameObject enemyInfoPanel;
        [SerializeField] private TMP_Text enemyInfoText;
        [SerializeField] private Image enemyHealthFill;

        [Header("LogUI")]
        [SerializeField] private GameObject logUI;
        [SerializeField] private TMP_Text logTextPrefab;
        private Queue<GameObject> logQueue = new Queue<GameObject>();
        private int maxLogs = 6;

        // Game State
        [Header("Game State")]
        public bool isPaused;                       // Tracks if the game is paused
        string goalCount;                              // Track the remaining goal

        // Level Management
        public string[] levels;    // Array of level names
        private int currentLevelIndex;              // Index of the current level

        public bool gameOver;




        // Awake is called before the first frame update
        void Awake() {
            // Singleton pattern to ensure only one instance of GameManager
            if (instance == null) {
                instance = this;
                // DontDestroyOnLoad(gameObject);      // Persist across scenes
                LoadScenesFromBuildSettings();      // Load the scenes from the build settings
            } else {
                Destroy(gameObject);                // Destroy any additional instances
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Find and assign the Player and its scripts (for game play scenes only)
            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }

            //do not move
            string currentSceneName = SceneManager.GetActiveScene().name;
            currentLevelIndex = System.Array.IndexOf(levels, currentSceneName);
            Debug.Log(currentLevelIndex);
            if (currentLevelIndex == -1)
            {
                Debug.LogError("Current scene is not in levels array: " + currentSceneName);
                currentLevelIndex = 0;
            }

            float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
            VolumeSlider.value = savedVolume;

            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            VolumeMusicSlider.value = savedMusicVolume;

            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            VolumeSFXSlider.value = savedSFXVolume;

            // Set the volume based on slider
            bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
            AudioManager.Instance.ToggleMute(isMuted);
            muteToggle.isOn = isMuted;

            VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            VolumeMusicSlider.onValueChanged.AddListener(OnVolumeChanged);
            VolumeSFXSlider.onValueChanged.AddListener(OnVolumeChanged);
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

            //StartCoroutine(VerifySpellNameText());

            if (enemyInfoPanel != null)
            {
                enemyInfoPanel.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Toggle Pause menu with the "Cancel" button (default is "Escape")
            if (Input.GetButtonDown("Cancel"))
            {
                if (menuActive == null && isPaused == false)
                {
                    StatePause();
                    menuActive = menuPause;
                    menuActive.SetActive(true);

                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayMenuMusic();  // This will play the menu music
                    }

                }
                else if (menuActive == menuPause && isPaused == true)
                {
                    StateUnpause();
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.CloseMenu();  // Resumes exploration music
                    }
                }
            }
        }

        public void OpenOptionsMenu()
        {
            if (menuActive != null)
            {
                menuActive.SetActive(false);
            }
            menuActive = menuOptions;
            menuActive.SetActive(true);
        }

        public void CloseOptionsMenu()
        {
            if (menuActive != null)
            {
                menuActive.SetActive(false);
            }
            menuActive = menuPause;
            menuActive.SetActive(true);
        }

        public void UpdateEnemyInfo(string enemyType, float healthPercent)
        {
            // Called whenever an enemy takes damage
            if (enemyInfoPanel != null && !enemyInfoPanel.activeSelf)
            {
                enemyInfoPanel.SetActive(true);
            }
            if (enemyInfoText != null)
            {
                enemyInfoText.text = enemyType;
            }
            if (enemyHealthFill != null)
            {
                enemyHealthFill.fillAmount = Mathf.Clamp01(healthPercent);
            }
        }

        public void HideEnemyInfo()
        {
            // Hides the name once an enemy dies
            if (enemyInfoPanel !=null)
            {
                enemyInfoPanel.SetActive(false);
            }
        }

        //private IEnumerator VerifySpellNameText()
        //{
        //    yield return new WaitForSeconds(1f);
        //    if (spellNameText == null)
        //    {
        //        Debug.LogWarning("SpellNameText is not assigned in the Inspector");
        //    }
        //}

        public void UpdateSpellInfo(string spellName, string spellDiscription, Sprite spellIcon)
        {
            spellNameText.text = spellName;
            spellDiscriptionText.text = spellDiscription;
            spellImage.sprite = spellIcon;
        }

        public void UpdateHealthValue(string health)
        {
            healthLevelText.text = health;
        }

        public void UpdateManaValue(string mana)
        {
            manaLevelText.text = mana;
        }

        // Pause / Unpause Methods
        public void StatePause() {
            isPaused = !isPaused;                       // sets isPaused to the opposite of what it currently is
            Time.timeScale = 0;                         // Freeze time when paused
            goalDescription.enabled = false;
            goalText.enabled = false;

        }

        public void StateUnpause() {
            isPaused = !isPaused;                       // sets isPaused to the opposite of what it currently is
            Time.timeScale = 1;                         // Unfreeze time when unpaused
            goalDescription.enabled = true;
            goalText.enabled = true;
            if (menuActive != null) {
                menuActive.SetActive(false);            // Hide the active menu
                menuActive = null;                      // Clear the active menu
            }
            if (AudioManager.Instance != null) {
                AudioManager.Instance.CloseMenu();
            }
        }

        // Goal Management
        public void UpdateGameGoal() {
            // Update the goal count
     
            
            goalText.text = System.Array.IndexOf(levels, SceneManager.GetActiveScene().name).ToString() + "/8";       // Update the UI to display goal text

            // check if the player has complete all goals
            //if (goalCount <= 0) {
            //    StatePause();                               // Pause the game

            //    if (currentLevelIndex + 1 < levels.Length) {
            //        if (menuNextLevel != null) {
            //            Debug.Log("Displaying Next Level Menu.");
            //            menuActive = menuNextLevel;                     // Set the next level menu as active
            //            menuActive.SetActive(true);                     // Display the next level menu
            //        } else {
            //            Debug.LogError("Next Level Menu is not assigned in the GameManager.");
            //        }
            //    } else {
            //        if (menuWin != null) {
            //            Debug.Log("Displaying Win Menu.");
            //            menuActive = menuWin;                           // Set the win menu as active
            //            menuActive.SetActive(true);                     // Display the win menu
            //        } else {
            //            Debug.LogError("Win Menu is not assigned in the GameManager.");
            //        }
            //    }
            //}
        }

        public IEnumerator GameOver() {
            gameOver = true;                    // Set the game over flag
            menuActive = menuLose;              // Set the lose menu as active
            menuActive.SetActive(true);         // Display the lose menu
            yield return new WaitForSeconds(1); // Wait for 1 second
            StatePause();                       // Pause the game

            StopCoroutine(GameOver());          // Stop the coroutine
        }

        // Level Management Methods
        private void LoadScenesFromBuildSettings() {
            int sceneCount = SceneManager.sceneCountInBuildSettings;        // Get Total Scenes in Build Settings
            levels = new string[sceneCount];                                // Create a new string array to store the scene names

            for (int i = 0; i < sceneCount; i++) {
                // Extract name from path
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                levels[i] = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            }
        }

        public void LoadLevel(int levelIndex) {
            if (levelIndex >= 0 && levelIndex < levels.Length) {
                currentLevelIndex = levelIndex;
              

                if (AudioManager.Instance != null) {
                    AudioManager.Instance.CloseMenu(); // ensures exploration music resumes when restarting or switching levels. 
                }
                SceneManager.LoadScene(levels[levelIndex]);         // Load the level
                StateUnpause();                                     // Unpause the game
               

            }
            else {
                Debug.LogError("Invalid level index: " + levelIndex);
            }
        }

        public void LoadNextLevel() {
            // Only proceed if there are more levels in the array
            if (currentLevelIndex + 1 < levels.Length) {
                currentLevelIndex++;        // Increment the current level index
                
                // skip invalid levels like "Main Menuu"
                while (currentLevelIndex < levels.Length && levels[currentLevelIndex] == "MainMenu") {
                    Debug.Log($"Skipping Main Menu at index {currentLevelIndex}");
                    currentLevelIndex++;    // Increment the current level index and skip main menu
                }

                // Ensure there are still valid levels to load
                if (currentLevelIndex < levels.Length) {
                    Debug.Log($"Loading Level: {levels[currentLevelIndex]} (Index: {currentLevelIndex}).");
                    LoadLevel(currentLevelIndex);   // Load the next level
                } else {
                    // No More Valid Levels
                    Debug.Log("No valid Levels remaining. Showing win menu");
                    ShowWinMenu();
                }
            } else {
                // Already at the last level - Show Win Menu
                Debug.Log("Already at the last level. Showing win menu.");
                ShowWinMenu();
            }
        }

        // Win Menu Function
        private void ShowWinMenu() {
            StatePause();                       // Pause the game
            if (menuWin != null) {
                menuActive = menuWin;           // Set the win menu as active
                menuActive.SetActive(true);     // Display the win menu
            } else {
                Debug.LogError("Win Menu is not assigned in the GameManager.");
            }
        }

        public void LoadMainMenu() {
            // Reset Game State
            Time.timeScale = 1;                 // Unfreeze time
            isPaused = false;                   // UnPause the game
            menuActive = null;                  // Clear the active menu

            // Reset the cursor for Main Menu
            Cursor.visible = true;                  // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor

            // Load the Main Menu Scene
            SceneManager.LoadScene("MainMenu");

            Debug.Log("Loaded Main Menu, time scale reset, and cursor restored.");
        }
        private void OnVolumeChanged(float volume) {
            if (!AudioManager.Instance.IsMuted()) {
                AudioManager.Instance.SetVolume(volume);
                PlayerPrefs.SetFloat("Volume", volume);
            }
        }
        private void OnVolumeMusicChanged(float volume)
        {
            if (!AudioManager.Instance.IsMuted())
            {
                AudioManager.Instance.SetMusicVolume(volume);
                PlayerPrefs.SetFloat("MusicVolume", volume);
            }
        }
        private void OnVolumeSFXChanged(float volume)
        {
            if (!AudioManager.Instance.IsMuted())
            {
                AudioManager.Instance.SetSFXVolume(volume);
                PlayerPrefs.SetFloat("SFXVolume", volume);
            }
        }

        private void OnMuteToggleChanged(bool isMuted) {
            AudioManager.Instance.ToggleMute(isMuted);
            PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        }


        public void AddLog(string message)
        {

            // Create the new log text object
            TMP_Text logText = Instantiate(logTextPrefab, logUI.transform);
            logText.text = message;
            logQueue.Enqueue(logText.gameObject); // Enqueue the GameObject, not the TMP_Text component

            // Check if we've exceeded the maximum number of logs
            if (logQueue.Count > maxLogs)
            {
                GameObject oldestLog = logQueue.Dequeue(); // Remove the oldest log from the queue
                Destroy(oldestLog); // Destroy the oldest log GameObject
            }

            StartCoroutine(FadeAndDestroyLog(logText)); //start the coroutine to fade.
        }

        private IEnumerator FadeAndDestroyLog(TMP_Text logText)
        {
            float displayDuration = 3.0f; // Time to display the log before fading
            float fadeDuration = 2.0f;    // Duration of the fade-out effect

            // Wait for the display duration before starting the fade
            yield return new WaitForSeconds(displayDuration);


            float elapsedTime = 0f;
            Color originalColor = logText.color;

            // Fade out loop
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                logText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null; // Wait for the next frame
            }

            // Ensure fully transparent
            logText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

            //Before destroying, check if this gameobject is in queue and dequeue.
            if (logQueue.Contains(logText.gameObject))
            {
                // Create a temporary list to hold the elements we want to keep.
                List<GameObject> tempList = new List<GameObject>();

                // Dequeue elements until the queue is empty.
                while (logQueue.Count > 0)
                {
                    GameObject currentLog = logQueue.Dequeue();
                    //If it is not equal, add to the temporary list.
                    if (currentLog != logText.gameObject)
                    {
                        tempList.Add(currentLog);
                    }
                }

                // Add all of temp list back to the queue.
                foreach (GameObject log in tempList)
                {
                    logQueue.Enqueue(log);
                }
            }

            // Destroy the object
            Destroy(logText.gameObject);
        }
    }
}