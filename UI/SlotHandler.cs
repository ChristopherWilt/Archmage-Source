using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Archmage.UI {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public class SlotHandler : MonoBehaviour, IDropHandler {
        [SerializeField]
        public string slotType; // "Equipped" or "Stored".  Set this in the Inspector!

        public void OnDrop(PointerEventData eventData) {
            Debug.Log("Dropped on slot" + slotType); //for testing if drop is valid.
        }
    }
}