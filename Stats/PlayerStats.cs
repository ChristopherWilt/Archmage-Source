using Archmage.Control;
using Archmage.Spells;
using System.Collections;
using UnityEngine;

namespace Archmage.Stats {
    public class PlayerStats : MonoBehaviour {
        #region Serialized Fields
        [Header("----- Player Stats -----")]
        [Tooltip("Players health")]
        [Range(0, 100)][SerializeField] int HP;
        [Tooltip("Players mana")]
        [Range(0, 100)][SerializeField] int mana;

        [Header("----- Regen Settings -----")]
        [Tooltip("The amount of mana regenerated per mana regen tick")]
        [SerializeField] int manaRegen;
        [Tooltip("The time before mana regen starts ticking")]
        [SerializeField] int PauseManaRegenTime;
        [Tooltip("The delay between each mana regen tick")]
        [SerializeField] int manaRegenDelay;
        [Tooltip("The amount of health regenerated per health regen tick")]
        [SerializeField] int healthRegen;
        [Tooltip("The delay between each health regen tick")]
        [SerializeField] int healthRegenTime;
        #endregion

        #region Private Fields
        int HPOriginal;
        int maxMana;
        int currentMana;

        bool isRegeneratingMana = false;
        bool canRegenerateMana = true;
        bool isRegeneratingHealth = false;

        float lastDamagetime = 0f;
        bool isUnderDot = false;
        readonly float healDelayAfterDamage = 5f;
        PlayerController playerController;
        #endregion

        #region Properties
        public int Mana {
            get => mana;
            set => mana = Mathf.Clamp(value, 0, 100);
        }

        public int MaxMana {
            get => maxMana;
            set => maxMana = value;
        }

        public int Health {
            get => HP;
            set => HP = Mathf.Clamp(value, 0, 100);
        }

        public int MaxHealth {
            get => HPOriginal;
            set => HPOriginal = value;
        }

        public bool CanRegenerateMana(float lastCastTime) =>
            !isRegeneratingMana && (Time.time - lastCastTime >= PauseManaRegenTime);
        #endregion

        #region Initialization
        public void Initialize(PlayerController _playerController) {
            playerController = _playerController;

            HPOriginal = HP;
            maxMana = mana;
        }
        #endregion


        #region Public Methods 
        public void RestoreHealth(int amount) {
            HP += amount;
            HP = Mathf.Clamp(HP, 0, HPOriginal);
            playerController.PlayerUI.UpdatePlayerUI();
        }

        public void RestoreMana(int amount) {
            mana += amount;
            mana = Mathf.Clamp(mana, 0, maxMana);
            playerController.PlayerUI.UpdatePlayerUI();
        }

        public void StartHealthRegen() {
            if (!isRegeneratingHealth)
                StartCoroutine(RegenHealth());
        }

        public void StartManaRegen() {
            if (!isRegeneratingMana && canRegenerateMana)
                StartCoroutine(RegenMana());
        }
        #endregion

        #region Private Coroutines
        IEnumerator RegenHealth() {
            isRegeneratingHealth = true;

            while (HP < HPOriginal) {
                // stop healing if the player takes damage or is under a damage over time effect
                if (Time.time - lastDamagetime < healDelayAfterDamage || isUnderDot)
                {
                    isRegeneratingHealth = false;
                    playerController.PlayerUI.StopHealEffect();
                    yield break;
                }

                    int lostHealth = HPOriginal - HP;
                yield return new WaitForSeconds(healthRegenTime);
                HP += lostHealth <= healthRegen ? lostHealth : healthRegen;

                playerController.PlayerUI.PulseHealPanel();
                playerController.PlayerUI.UpdatePlayerUI();
            }
            playerController.PlayerUI.StopHealEffect();
            isRegeneratingHealth = false;
        }

        IEnumerator RegenMana() {
            isRegeneratingMana = true;

            while (mana < maxMana && canRegenerateMana) {
                int lostMana = maxMana - mana;
                mana += Mathf.Min(lostMana, manaRegen);

                MagicManager.Instance.SetMana(mana);
                playerController.PlayerUI.UpdatePlayerUI();

                yield return new WaitForSeconds(manaRegenDelay);
            }
            isRegeneratingMana = false;
        }

        public void ResetStats() {
            HP = HPOriginal;
            mana = maxMana;

            playerController.PlayerUI.UpdatePlayerUI();
        }
        #endregion
    }
}