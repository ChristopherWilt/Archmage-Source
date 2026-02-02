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
    public class EquippedSlot : MonoBehaviour, IPointerClickHandler {
        // SLOT APPEARANCE //
        [SerializeField] private Image slotImage;
        [SerializeField] private TMP_Text slotName;

        // SLOT DATA //
        [SerializeField] ItemType slotType = new ItemType();
        [SerializeField] private GameObject imageObject;
        [SerializeField] private Image itemImage;
        private Sprite itemSprite;
        private string itemName;
        private int itemDamage;

        private string itemDescription;

        // OTHER //
        private bool SlotInUse;
        [SerializeField] public GameObject selectedShader;
        [SerializeField] public bool thisItemSelected;

        [SerializeField] private Sprite emptySprite;

        private Inventory inventoryManager;
        PlayerController playerController;


        private void Start() {
            inventoryManager = GameObject.Find("Player").GetComponent<Inventory>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        }

        public void EquipGear(Sprite itemSprite, string itemName, string itemDescription) {
            // If a slot is already in use, unequip it

            if (SlotInUse) {
                UnEquipGear();
            }

            // Updates Image
            this.itemSprite = itemSprite;
            slotImage.sprite = this.itemSprite;
            this.imageObject.SetActive(true);

            slotName.enabled = false;

            // Update Data
            this.itemName = itemName;
            this.itemDescription = itemDescription;

            SlotInUse = true;
        }
        private void UnEquipGear() {
            inventoryManager.DeselectAllSlots();

            inventoryManager.AddToStored(new Item(itemName, itemSprite, 1, itemDamage, slotType, itemDescription));

            if (slotType == ItemType.Boots)
            {
                playerController.PlayerMovement.speed = 4;
            }

            // Update Image
            itemImage.sprite = emptySprite;
            this.itemSprite = emptySprite;
            this.imageObject.SetActive(false);
            slotImage.sprite = this.emptySprite;
            slotName.enabled = true;
            SlotInUse = false;

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
            if (thisItemSelected && SlotInUse) {
                UnEquipGear();
            } 
            else {
                inventoryManager.DeselectAllSlots();
                selectedShader.SetActive(true);
                thisItemSelected = true;
            }
        }

        public void UseSlot()
        {
            if (slotType == ItemType.ManaPotion || slotType == ItemType.HealthPotion)
            {
                if (playerController.PlayerStats.Health < playerController.PlayerStats.MaxHealth || playerController.PlayerStats.Mana < playerController.PlayerStats.MaxMana)
                {
                    inventoryManager.UseItem(new Item(itemName, itemSprite, 1, itemDamage, slotType, itemDescription));
                    itemImage.sprite = emptySprite;
                    this.itemSprite = emptySprite;
                    this.imageObject.SetActive(false);
                    slotImage.sprite = this.emptySprite;
                    slotName.enabled = true;
                    SlotInUse = false;
                }
            }
        }

        private void OnRightClick() {
            if (SlotInUse)
            {
                UseSlot();
            }
        }
    }
}