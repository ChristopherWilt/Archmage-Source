using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Archmage.Items;
using Archmage.Control;



namespace Archmage.UI {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler {
        //=======Item Data=======\\
        public string itemName;
        public string itemDescription;
        public int itemDamage;
        public int quantity;
        public ItemType itemType;
        public Sprite itemSprite;
        public Sprite emptySprite;
        public bool isFull;

        [SerializeField]
        private int maxNumberOfItems = 20;

        //=======Item Slot=======\\
        [SerializeField]
        private Image itemImage;
        [SerializeField] private GameObject imageObject;
        private Inventory inventoryManager;
        [SerializeField] private TMP_Text quantityTxt;

        //=======Equipped Slot=======\\
        [SerializeField] private EquippedSlot ArmorSlot, BootSlot, WeaponSlot, AbilitySlot, PotionSlot;

        //=======Item Description=======\\
        public Image itemDescriptionImage;

        [SerializeField] public GameObject selectedShader;
        public bool thisItemSelected;


        PlayerController playerController;



        private void Awake()
        {
            inventoryManager = GameObject.Find("Player").GetComponent<Inventory>();
        }

        private void Start() {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        public int AddItem(string name, Sprite icon, int quantity, int damage, string description, ItemType type) {
            if (isFull) {
                return quantity;
            }

            this.itemName = name;
            this.itemDamage = damage;
            this.itemType = type;
            this.itemDescription = description;
            this.isFull = true;

            this.itemSprite = icon;
            itemImage.sprite = icon;
            this.itemImage.enabled = true;
            this.imageObject.SetActive(true);

            this.quantity += quantity;
            if (this.quantity >= maxNumberOfItems) {
                quantityTxt.text = maxNumberOfItems.ToString();
                quantityTxt.enabled = true;
                isFull = true;

                // Return the left over items
                int extraItems = this.quantity - maxNumberOfItems;
                this.quantity = maxNumberOfItems;
                return extraItems;
            }

            // Update the quantity text
            quantityTxt.text = this.quantity.ToString();
            quantityTxt.enabled = true;

            return 0;
        }

        public void EquipGear() {
            if (itemType == ItemType.Weapon) {
                WeaponSlot.EquipGear(itemSprite, itemName, itemDescription);
                GameManager.instance.AddLog(itemName + " Was Equipped");
                OnCursorExit();
                EmptySlot();
            }

            if (itemType == ItemType.Ability) {
                AbilitySlot.EquipGear(itemSprite, itemName, itemDescription);
                GameManager.instance.AddLog(itemName + " Was Equipped");
                OnCursorExit();
                EmptySlot();
            }

            if (itemType == ItemType.HealthPotion || itemType == ItemType.ManaPotion) {
                PotionSlot.EquipGear(itemSprite, itemName, itemDescription);
                GameManager.instance.AddLog(itemName + " Was Equipped");
                OnCursorExit();
                EmptySlot();
            }

            if (itemType == ItemType.Armor) {
                ArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
                GameManager.instance.AddLog(itemName + " Was Equipped");
                OnCursorExit();
                EmptySlot();
            }

            if (itemType == ItemType.Boots) {
                BootSlot.EquipGear(itemSprite, itemName, itemDescription);
                GameManager.instance.AddLog(itemName + " Was Equipped");
                playerController.PlayerMovement.speed = 6;
                OnCursorExit();
                EmptySlot();
            }
        }

        public void UseSlot()
        {
            if (itemType == ItemType.HealthPotion || itemType == ItemType.ManaPotion)
            {
                if (playerController.PlayerStats.Health < playerController.PlayerStats.MaxHealth || playerController.PlayerStats.Mana < playerController.PlayerStats.MaxMana)
                {
                    inventoryManager.UseItem(new Item(itemName, itemSprite, 1, itemDamage, itemType, itemDescription));
                    this.quantity--;
                    quantityTxt.text = this.quantity.ToString();
                        OnCursorExit();
                        EmptySlot();
                    this.imageObject.SetActive(false);
                    selectedShader.SetActive(false);
                    thisItemSelected = false;
                    Debug.Log("Item Potion and reset slot");
                }
            }


        }

        public void UseKey()
        {
            if (itemType == ItemType.ChestKey || itemType == ItemType.smallChestKey || itemType == ItemType.DoorKey)
            {
                this.quantity--;
                quantityTxt.text = this.quantity.ToString();
                EmptySlot();
                OnCursorExit();
                this.imageObject.SetActive(false);
                selectedShader.SetActive(false);
                thisItemSelected = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnLeftClick();
            }

            if (eventData.button == PointerEventData.InputButton.Right) {
                OnRightClick();
            }
        }
        private void OnLeftClick() {
            if (thisItemSelected && isFull) {
                EquipGear();
            } 
            else 
            {
                inventoryManager.DeselectAllSlots();
                selectedShader.SetActive(true);
                thisItemSelected = true;
            }
        }
        private void OnRightClick()
        {
            if (thisItemSelected && isFull)
            {
                UseSlot();
            }
        }

        public void OnCursorEnter() {
            if (isFull)
                inventoryManager.OnHoverOverItem(itemName, itemDescription, transform.position);
        }

        public void OnCursorExit() {
            if (isFull)
                inventoryManager.OnHoverOverItemExit();
        }

        private void EmptySlot() {
            quantityTxt.enabled = false;
            itemImage.sprite = emptySprite;
            isFull = false;
            this.itemSprite = emptySprite;
            this.itemImage.enabled = false;
            this.imageObject.SetActive(false);
            this.itemName = "";
            this.itemDescription = "";
            this.itemDamage = 0;
            this.itemType = ItemType.Misc;
            this.quantity = 0;
            this.itemDescriptionImage.enabled = false;
        }

    }
}
