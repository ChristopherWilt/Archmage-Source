using Archmage.Items;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public List<LootStats> lootList = new List<LootStats>();

    LootStats GetDroppedItem()
    {
        int RandomNumber = Random.Range(1, 101);
        List<LootStats> possibleItems = new List<LootStats>();
        foreach (LootStats item in lootList)
        {
            if (RandomNumber <= item.DropChance)
            {
                possibleItems.Add(item);
                Debug.Log("Item added to possibleItems: " + item.lootName);
            }
        }
        if (possibleItems.Count > 0)
        {
            LootStats droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            Debug.Log("Item dropped: " + droppedItem.lootName);
            return droppedItem;
        }
        Debug.Log("No item dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPos)
    {
        LootStats droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(dropItemPrefab, spawnPos, Quaternion.identity);
            lootGameObject.GetComponent<MeshFilter>().mesh = droppedItem.lootMesh;
            lootGameObject.GetComponent<MeshRenderer>().materials = new Material[] { droppedItem.lootMaterial[0] };

            lootGameObject.GetComponent<Item>().itemName = droppedItem.lootName;
            lootGameObject.GetComponent<Item>().itemIcon = droppedItem.lootIcon;
            lootGameObject.GetComponent<Item>().itemType = droppedItem.lootType;
            lootGameObject.GetComponent<Item>().description = droppedItem.lootDescription;
            lootGameObject.GetComponent<Item>().quantity = droppedItem.lootQuantity;

            Debug.Log("Item dropped: " + droppedItem.lootName);
        }
    }
}
