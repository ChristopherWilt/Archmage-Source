// ItemSpawner.cs
using Archmage.Items;
using UnityEngine;

namespace Archmage.UI {
    public class ItemSpawner : MonoBehaviour {
        public GameObject itemPrefab; // Drag your item prefab here
        public Inventory inventory; // Drag your Inventory GameObject here

        // Example, Array of different images to use when spawning
        public Sprite[] differentSprites;

        [System.Obsolete]
        void Start() {
            if (inventory == null) {
                inventory = FindObjectOfType<Inventory>();
            }
            // Instantiate the item prefab
            GameObject newItemGO = Instantiate(itemPrefab);

            // Get the ItemDrag component
            ItemDrag itemDrag = newItemGO.GetComponent<ItemDrag>();

            if (itemDrag != null && itemDrag.item != null) {
                // Add to inventory
                if (inventory != null) {
                    inventory.AddToStored(itemDrag.item);
                } else {
                    Debug.LogError("Inventory is null.");
                }

            } else {
                Debug.LogError("ItemDrag component or Item is missing on the prefab!");
                Destroy(newItemGO); // Clean up if something's wrong
            }
            Destroy(newItemGO); //remove from scene.
        }
    }
}
