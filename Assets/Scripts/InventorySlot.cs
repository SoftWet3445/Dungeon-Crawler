using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemType slotType;

    public ObjectItemBase item;

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
        if (InventorySystem.instance.itemArray[slotID] == null /*&& itemIcon.enabled*/) { itemIcon.enabled = false; }

        //if (slotID == 0 || slotID == 1) { Debug.Log(itemIcon.enabled); }
        item = InventorySystem.instance.itemArray[slotID];

        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (item == null)
        {
            switch (slotType)
            {
                case ItemType.weapon:
                    PlayerWeaponHandler weaponHandler = FindObjectOfType<PlayerWeaponHandler>();
                    if (weaponHandler != null)
                    { weaponHandler.currentWeapon = null; weaponHandler.UpdateHeldWeapon(); }
                    break;
                case ItemType.shield:
                    break;
                case ItemType.ammo:
                    break;
                case ItemType.trinket:
                    break;
                case ItemType.head:
                    break;
                case ItemType.body:
                    break;
                case ItemType.legs:
                    break;
            }
        }
        else if (item != null)
        {
            switch (slotType)
            {
                case ItemType.weapon:
                    PlayerWeaponHandler weaponHandler = FindObjectOfType<PlayerWeaponHandler>();
                    if (weaponHandler != null && item != null)
                    { weaponHandler.currentWeapon = item as ObjectItemWeapon; weaponHandler.UpdateHeldWeapon(); }
                    break;
                case ItemType.shield:
                    break;
                case ItemType.ammo:
                    break;
                case ItemType.trinket:
                    break;
                case ItemType.head:
                    break;
                case ItemType.body:
                    break;
                case ItemType.legs:
                    break;
            }
        }
    }

    public void UpdateItem(ObjectItemBase item)
    {
        // Exit the function if the item is null, I hope this works
        if (item == null) { return; }

        // Update the slot's icon the new item's sprite
        if (item != null || item.itemSprite != null) { itemIcon.sprite = item.itemSprite; } 
        else { return; } // Maybe redundant but I am incredibly sick of this null reference error

        if (InventorySystem.instance.itemArray[slotID] != null /*&& !itemIcon.enabled*/) { itemIcon.enabled = true; }
    }

    public void SendIDToInventory()
    {
        if (item == null && InventorySystem.instance.swapSlotList.Count < 1) { return; }

        Debug.Log("Send ID to inventory");
        // send this slots id to the main inventory system, used for swapping objects
        InventorySystem.instance.swapSlotList.Add(this);
    }
}
