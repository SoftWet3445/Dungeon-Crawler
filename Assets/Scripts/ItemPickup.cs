using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] ObjectItemBase item;

    public void Interact()
    {
        if (InventorySystem.instance != null) 
        { 
            // Add the item to the inventorys
            InventorySystem.instance.AddItemToInventory(item);
            // Destroy the object
            Destroy(this.gameObject);
        }
    }
    
    public string InteractDescription()
    {
        return "Picked up item " + item.itemName;
    }
}
