//// DragHandler.cs
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Archmage.UI {
    //public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    //{
    //    private Transform originalParent;
    //    [SerializeField]
    //    private Canvas canvas;
    //    private Item myItem;
    //    public Inventory inventory; // Use direct reference

    //    private void Start()
    //    {
    //        canvas = GetComponentInParent<Canvas>();
    //        myItem = GetComponent<ItemDrag>().item;
    //        if (canvas == null)
    //        {
    //            Debug.LogError("DragHandler must be a child of a Canvas!");
    //        }
    //        if (inventory == null) //check if its in the inspector
    //        {
    //            Debug.LogError("DragHandler need Inventory reference.");
    //        }
    //    }

    //    public void OnBeginDrag(PointerEventData eventData)
    //    {
    //        originalParent = transform.parent;
    //        transform.SetParent(canvas.transform);
    //        transform.SetAsLastSibling();
    //        GetComponent<CanvasGroup>().blocksRaycasts = false;
    //    }

    //    public void OnDrag(PointerEventData eventData)
    //    {
    //        transform.position = eventData.position;
    //    }

    //    public void OnEndDrag(PointerEventData eventData)
    //    {
    //        transform.SetParent(originalParent);
    //        GetComponent<CanvasGroup>().blocksRaycasts = true;

    //        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;

    //        if (dropTarget != null)
    //        {

    //            SlotHandler slotComponent = dropTarget.GetComponent<SlotHandler>();
    //            if (slotComponent != null)
    //            {
    //                if (slotComponent.slotType == "Equipped")
    //                {
    //                    // Attempt to equip the item
    //                    if (inventory.RemoveFromStored(myItem))
    //                    {//if item is in stored
    //                        inventory.EquipItem(myItem);
    //                    }

    //                }
    //                else if (slotComponent.slotType == "Stored")
    //                {
    //                    // Attempt to unequip and store the item
    //                    if (inventory.RemoveFromEquipped(myItem))
    //                    { //if item is already equipped
    //                        inventory.AddToStored(myItem);
    //                    }

    //                }
    //            }
    //            else
    //            {
    //                //if not over a slot
    //            }
    //        }

    //        // Reset position to its original position within the slot
    //        transform.localPosition = Vector3.zero;
    //    }
    //}
// }