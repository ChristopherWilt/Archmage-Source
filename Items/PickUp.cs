using Archmage.Control;
using UnityEngine;

namespace Archmage.Items {
    public class PickUp : MonoBehaviour {
        public enum PickupType {
            Health, Mana
        }
        [SerializeField] PickupType type;
        [SerializeField] int amount;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null) {
                    if (type == PickupType.Health && playerController.PlayerStats.Health < playerController.PlayerStats.MaxHealth) {
                        playerController.PlayerStats.RestoreHealth(amount);
                        Destroy(gameObject);
                    } else if (type == PickupType.Mana && playerController.PlayerStats.Mana < playerController.PlayerStats.MaxMana) {
                        playerController.PlayerStats.RestoreMana(amount);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
