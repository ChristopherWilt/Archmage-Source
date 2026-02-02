using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Archmage.AI;

namespace Archmage.UI {
    public class TutorialManager : MonoBehaviour {
        [Header("UI Elements")]
        public TextMeshProUGUI tutorialText;
        public GameObject tutorialPanel;
        public Button skipButton;

        private int currentStep = 0;
        private bool tutorialActive = true;

        // Updated tutorial instructions:
        // Step 0: Use WSAD to Move
        // Step 1: Press SHIFT to sprint.
        // Step 2: Left Click to cast a spell.
        // Step 3: Right Click for a charged attack.
        // Step 4: Press Q/E to zoom the camera in/out.
        // Step 5: Press T to use your special ability: Teleport.
        // Step 6: Use the Scroll Wheel to switch spells.
        // Step 7: Defeat the enemy using your spells! (advanced via an event)
        private string[] tutorialSteps = {
        "Use WSAD to Move",
        "Press SHIFT to sprint",
        "Left Click to cast a spell",
        "Press tab to open inventory. right click potions to use them.",
        "Press F to open chests if you have keys.",
        "Press Q/E to zoom the camera in/out",
        "Press T to use your special ability: Teleport",
        "Use the Scroll Wheel to switch spells",
        "Defeat the enemy using your spells!"
    };

        void Start() {
            tutorialPanel.SetActive(true);
            tutorialText.text = tutorialSteps[currentStep];
            EnemyAI.OnEnemyDefeated += HandleEnemyDefeated;
        }

        void Update() {
            if (!tutorialActive)
                return;

            // Step 0: Movement - check if any of the WASD keys are pressed.
            if (currentStep == 0 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
                NextStep();

            // Step 1: Sprint - check for either SHIFT key.
            if (currentStep == 1 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
                NextStep();

            // Step 2: Cast a spell - check for left mouse button click.
            if (currentStep == 2 && Input.GetMouseButtonDown(0))
                NextStep();

            // Step 3: Inventory - check for tab pressed.
            if (currentStep == 3 && Input.GetKeyDown(KeyCode.Tab))
                NextStep();
            // Step 4: Keys - check for F pressed.
            if (currentStep == 4 && Input.GetKeyDown(KeyCode.F))
                NextStep();

            // Step 5: Zoom camera - check if Q or E is pressed.
            if (currentStep == 5 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
                NextStep();

            // Step 6: Special ability - check for T key.
            if (currentStep == 6 && Input.GetKeyDown(KeyCode.T))
                NextStep();

            // Step 7: Switch spells - check for scroll wheel movement.
            if (currentStep == 7 && Input.mouseScrollDelta.y != 0)
                NextStep();
        }

        void NextStep() {
            currentStep++;

            if (currentStep >= tutorialSteps.Length) {
                EndTutorial();
            } else {
                tutorialText.text = tutorialSteps[currentStep];
            }
        }

        // This method is called when an enemy is defeated.
        void HandleEnemyDefeated() {
            if (currentStep == 7)
                NextStep();
        }

        void SkipTutorial() {
            EndTutorial();
        }

        void EndTutorial() {
            tutorialActive = false;
            tutorialPanel.SetActive(false);
        }
    }
}