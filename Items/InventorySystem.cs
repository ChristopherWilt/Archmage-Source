// Inventory.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Archmage.UI;
using Archmage.Control;



namespace Archmage.Items {
    public class Inventory : MonoBehaviour {
        public Inventory Instance;
        public GameObject inventoryMenu;
        public ItemSlot[] storedSlots;
        public EquippedSlot[] EquippedSlot;

        public bool menuOpen;
        public ItemType itemType;

        // for OnHoverOverItem // 
        public Transform canvas;
        public GameObject itemInfoPrefab;
        private GameObject currentItemInfo = null;
        PlayerController playerController;

        private void Awake() {
            // Singleton pattern
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        public void Start() {
            // example of adding items to inventory
            //for (int i = 0; i < MaxStoredSlots; i++)
            //{
            //    Item item = new Item();
            //     item.icon = tempSprite; // Set the icon
            //AddToStored(item);
            // }

        }

        private void Update() {
            if (Input.GetButtonDown("Inventory") && menuOpen && GameManager.instance.isPaused == true) {
                GameManager.instance.StateUnpause();
                inventoryMenu.SetActive(false);
                menuOpen = false;
            } 

            else if (Input.GetButtonDown("Inventory") && !menuOpen && GameManager.instance.isPaused == false) 
            {
                inventoryMenu.SetActive(true);
                menuOpen = true;
                GameManager.instance.StatePause();
            }
        }

        public void AddToStored(Item item) {
            for (int i = 0; i < storedSlots.Length; i++) {
                if (storedSlots[i].isFull == false) {
                    storedSlots[i].AddItem(item.itemName, item.itemIcon, item.quantity, item.itemDamage, item.description, item.itemType);
                    GameManager.instance.AddLog(item.itemName + " added to inventory");
                    return;
                }
            }


        }

        public void EquipItem(Item item) {
            for (int i = 0; i < storedSlots.Length; i++) {
                if (storedSlots[i].thisItemSelected == true) {
                    storedSlots[i].EquipGear();
                    return;
                }
            }
        }

        public void UseItem(Item item) {
            // Add functionality for using an item, such as a health potion

            if (item.itemType == ItemType.HealthPotion)
            {
                playerController.PlayerStats.RestoreHealth(25);
                playerController.PlayerUI.UpdatePlayerUI();
                Debug.Log("Health Potion Used");
                GameManager.instance.AddLog(item.itemName + " used");
            }

            else if (item.itemType == ItemType.ManaPotion)
            {
                playerController.PlayerStats.RestoreMana(25);
                playerController.PlayerUI.UpdatePlayerUI();
                Debug.Log("Mana Potion Used");
                GameManager.instance.AddLog(item.itemName + " used");
            }

            else
            {
                GameManager.instance.AddLog(item.itemName + " can not be consumed or used");
                Debug.Log("Item is not a consumable or potion");
            }
        }


        public bool CanUseKey(ItemType type)
        {
            // Add functionality for using a key
            for (int i = 0; i < storedSlots.Length; i++)
            {
                if (storedSlots[i].itemType == ItemType.ChestKey && type == ItemType.ChestKey||
                    storedSlots[i].itemType == ItemType.smallChestKey && type == ItemType.smallChestKey || storedSlots[i].itemType == ItemType.DoorKey && type == ItemType.DoorKey)
                {
                    storedSlots[i].UseKey();
                    return true;
                }
            }
            // If the loop completes without finding a key, return false.
            Debug.Log("No Valid Key in Inventory");
            return false;
        }

        public void DeselectAllSlots() {
            for (int i = 0; i < storedSlots.Length; i++) {
                storedSlots[i].selectedShader.SetActive(false);
                storedSlots[i].thisItemSelected = false;
            }
            for (int i = 0; i < EquippedSlot.Length; i++) {
                EquippedSlot[i].selectedShader.SetActive(false);
                EquippedSlot[i].thisItemSelected = false;
            }
        }


        public void OnHoverOverItem(string itemName, string itemDescription, Vector2 buttonPos) {
            if (currentItemInfo != null) {
                Destroy(currentItemInfo.gameObject);
            }
            buttonPos.x -= 180;
            buttonPos.y -= 140;
            currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, canvas);
            currentItemInfo.GetComponent<ItemInfo>().SetInfo(itemName, itemDescription);
        }

        public void OnHoverOverItemExit() {
            if (currentItemInfo != null) {
                Destroy(currentItemInfo.gameObject);
            }
        }

    }
    public enum ItemType {
        Weapon,
        Armor,
        Boots,
        Consumable,
        HealthPotion,
        ManaPotion,
        Ability,
        Misc,
        ChestKey,
        smallChestKey,
        DoorKey
    };
}