using Archmage.Control;
using Archmage.Stats;
using System.Collections;
using UnityEngine;

namespace Archmage.UI {
    public class PlayerUIController : MonoBehaviour
    {
        #region Private Fields
        PlayerController playerController;
        #endregion

        #region Initialization
        public void Initialize(PlayerController _playerController)
        {
            playerController = _playerController;
            GameManager.instance.UpdateGameGoal();
        }
        #endregion

        #region UI Elements
        public GameObject healthWarningIndicator; // Exclemation Mark
        #endregion

        #region Public Methods
        public void UpdatePlayerUI()
        {
            float healthPercentage = (float)playerController.PlayerStats.Health / playerController.PlayerStats.MaxHealth;

            GameManager.instance.playerHPBar.fillAmount = healthPercentage;
            GameManager.instance.playerManaBar.fillAmount = (float)playerController.PlayerStats.Mana / playerController.PlayerStats.MaxMana;


            string manaValue = $"{playerController.PlayerStats.Mana} / {playerController.PlayerStats.MaxMana}";
            GameManager.instance.UpdateManaValue(manaValue);

            string healthValue = $"{playerController.PlayerStats.Health} / {playerController.PlayerStats.MaxHealth}";
            GameManager.instance.UpdateHealthValue(healthValue);

            // Show warning indicator if health is below 50%, hide otherwise
            if (healthWarningIndicator != null)
            {
                healthWarningIndicator.SetActive(healthPercentage < 0.5f);
            }
        }

        public void FlashDamagePanel()
        {
            StartCoroutine(FlashDamagePanelCoroutine());
        }

        public void PulseHealPanel()
        {
            StartCoroutine(PulseHealPanelCoroutine());
        }
        #endregion

        #region Private Coroutines
        IEnumerator FlashDamagePanelCoroutine()
        {
            GameManager.instance.damagePanel.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            GameManager.instance.damagePanel.SetActive(false);
        }

        private IEnumerator PulseHealPanelCoroutine()
        {
            CanvasGroup healPanel = GameManager.instance.healPanel.GetComponent<CanvasGroup>();

            if (healPanel == null)
            {
                Debug.LogError("HealPanel does not have a CanvasGroup component!");
                yield break;
            }

            healPanel.gameObject.SetActive(true);

            // Pulse effect (fade in and out)
            float duration = 3f;  // pulse length
            float maxAlpha = 0.5f;  // Max visibility of heal effect
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = (Mathf.Sin(elapsed * Mathf.PI / duration) + 1) / 2 * maxAlpha;  // Smooth pulse effect
                healPanel.alpha = alpha;
                yield return null;
            }
            while (healPanel.alpha > 0)
            {
                healPanel.alpha -= Time.deltaTime * 0.5f; // Adjust speed here
                yield return null;
            }

            healPanel.alpha = 0;
            healPanel.gameObject.SetActive(false);

            #endregion
        }
        public void StopHealEffect()
        {
            if (GameManager.instance.healPanel != null)
            {
                GameManager.instance.healPanel.SetActive(false);
            }
        }


    }
}
