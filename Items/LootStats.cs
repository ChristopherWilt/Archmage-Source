using Archmage.Items;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootStats : ScriptableObject
{
    // [SerializeField] public GameObject lootPrefab;
    [Header("--- Loot Asset ---")]
    [Space]

    [SerializeField] public Mesh lootMesh;
    [SerializeField] public List <Material> lootMaterial;

    [Space]
    [Header("--- Loot Stats ---")]
    [Space]

    [SerializeField] public int DropChance;
    [SerializeField] public string lootName;
    [SerializeField] public Sprite lootIcon;
    [SerializeField] public ItemType lootType;
    [SerializeField] public string lootDescription;
    [SerializeField] public int lootQuantity;


    public LootStats(Mesh mesh, List<Material> material, int dropChance, string name, Sprite icon, ItemType type, string description, int quantity)
    {
        lootMesh = mesh;
        lootMaterial = material;
        DropChance = dropChance;
        lootName = name;
        lootIcon = icon;
        lootType = type;
        lootDescription = description;
        lootQuantity = quantity;
    }
}
