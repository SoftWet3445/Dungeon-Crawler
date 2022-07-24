using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IS_InventorySlot : MonoBehaviour
{
    public ItemType slotType;

    public SO_ItemBase item;

    public int itemID; // Grabbed from the SO_itemDatabase
    public int slotID;

    public Image itemIcon;

    private void Awake()
    {
        itemIcon = this.transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        // Enable or disable the item icon if there is an item stored in this slot or not
        if (IS_InventoryBase.instance.itemArray[slotID] == null /*&& itemIcon.enabled*/) { itemIcon.enabled = false; }
        else if (IS_InventoryBase.instance.itemArray[slotID] != null /*&& !itemIcon.enabled*/) { itemIcon.enabled = true; }

        //if (slotID == 0 || slotID == 1) { Debug.Log(itemIcon.enabled); }
        item = IS_InventoryBase.instance.itemArray[slotID];
    }

    public void UpdateItem(SO_ItemBase item)
    {
        // Exit the function if the item is null, I hope this works
        if (item == null) { /*itemIcon.sprite = null;*/ return; }

        // Update the slot's icon the new item's sprite
        if (item != null || item.itemSprite != null) { itemIcon.sprite = item.itemSprite; } 
        else { return; } // Maybe redundant but I am incredibly sick of this null reference error
    }

    public void SendIDToInventory() 
    {
        if (item == null && IS_InventoryBase.instance.swapSlotList.Count < 1) { return; }
        // Send this slots ID to the main inventory system, used for swapping objects
        IS_InventoryBase.instance.swapSlotList.Add(this);
    }
}
