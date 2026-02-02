using UnityEngine;
using Archmage.Items;


public class Chests : MonoBehaviour, I_Interactable
{
    [SerializeField] private string _interactText = "Press E to Open Chest";
    [SerializeField] private bool _isOpen;
    [SerializeField] private Animator _animator;
    [SerializeField] private ItemType _keyType;
    [SerializeField] LootBag lootBag;

    private Inventory _inventory;
    public string InteractText => _interactText;

    public void Start()
    {
       _inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    public bool Interact(Interactor interactor)
    {
        if (_inventory != null)
        {
            if (_inventory.CanUseKey(_keyType))
            {
                _isOpen = true;
                lootBag.InstantiateLoot(transform.position);
                Debug.Log("Chest Opened");
                return true;
            }
            else
            {
                Debug.Log("You need a key to open this chest");
                return false;
            }
        }
        else
        {
            Debug.Log("Inventory not found");
            return false;
        }
    }
}
