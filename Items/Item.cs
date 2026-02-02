// Item.cs (Separate file is good practice)
using UnityEngine;
using UnityEngine.UI;

namespace Archmage.Items {
    [System.Serializable]
    public class Item : MonoBehaviour {
        [SerializeField]
        public string itemName;
        [SerializeField]
        public Sprite itemIcon;
        [SerializeField]
        public int itemDamage;
        [SerializeField]
        public ItemType itemType;
        [SerializeField]
        public string description;
        [SerializeField]
        public int quantity;
        Inventory inventoryManager;

        void Start() {
            inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

            if (inventoryManager == null) {
                Debug.LogError("Inventory Manager not found!  Make sure the Player has the Inventory script attached and is tagged 'Player'.");
            }
        }

        private void OnTriggerEnter(Collider collider) {
            if (collider.CompareTag("Player")) {
                inventoryManager.Instance.AddToStored(new Item(itemName, itemIcon, quantity, itemDamage, itemType, description));
                Destroy(gameObject);

            }
        }
        public Item(string name, Sprite icon, int quantity, int damage, ItemType type, string itemDescription) {
            this.itemName = name;
            this.itemIcon = icon;
            this.itemDamage = damage;
            this.itemType = type;
            this.quantity = quantity;
            this.description = itemDescription;
        }
        public Item() {
            this.itemName = "Default Item";
            this.itemIcon = null;
            this.itemDamage = 0;
            this.itemType = ItemType.Misc;
            this.quantity = 0;
            this.description = "";
        }

        public virtual string GetItemDescription() {
            return this.description;
        }
    }
}