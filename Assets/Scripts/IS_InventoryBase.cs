using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { weapon, shield, ammo, trinket, consumable, head, body, legs, undefined }
public class IS_InventoryBase : MonoBehaviour
{
    public static IS_InventoryBase instance;

    public int inventorySize = 20;
    public int specialSlots = 9;

    [SerializeField] private GameObject inventorySlotPrefab;

    [SerializeField] private IS_InventorySlot[] classSlotArray;
    public SO_ItemBase[] itemArray;
    [SerializeField] private List<GameObject> objectSlotList;

    [SerializeField] private SO_ItemBase testAddItem;

    public List<IS_InventorySlot> swapSlotList;

    private void Awake() => instance = this;

    private void Start()
    {
        // Instantiate arrays
        classSlotArray = new IS_InventorySlot[inventorySize + specialSlots];
        itemArray = new SO_ItemBase[inventorySize + specialSlots];
        // Instantiate lists
        objectSlotList = new List<GameObject>();
        swapSlotList = new List<IS_InventorySlot>();

        // Create the inventory
        DisplayInventory();
        // Add the special slots to the inventory
        AddSpecialSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild) { AddItemToInventory(testAddItem); }
        //if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild) { DestroyInventory(); }
        if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild) { DisplayInventory(); }

        if (swapSlotList.Count == 2) 
        {
            SwapItems(swapSlotList[0].slotID, swapSlotList[1].slotID);
        }

    }

    private void DisplayInventory()
    {
        // Try to destroy the inventory
        DestroyInventory();

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject newSlot = null;
            // Create an instance of the inventory slot object
            newSlot = Instantiate(inventorySlotPrefab, transform.position, Quaternion.identity, transform);
            // Add the inventory slot's class to the inventory slot array
            classSlotArray[i] = newSlot.GetComponent<IS_InventorySlot>();
            // Update the slot's slotID
            classSlotArray[i].slotID = i;
            classSlotArray[i].slotType = ItemType.undefined;

            // Add the instantiated game object to the object slot list
            objectSlotList.Add(newSlot);
            // Display the item in the slot if it exists
            if (itemArray[i] != null) { classSlotArray[i].UpdateItem(itemArray[i]); }
        }
    }

    private void AddSpecialSlots()
    {
        for (int i = 1; i < 10; i++)
        {
            // Create index a, being i + the invetory size minus one (arrays are zero indexed) to account for bulk slots
            int a = i + (inventorySize - 1);
            // Add the special inventory slot at index a 
            classSlotArray[a] = transform.parent.GetChild(i).GetComponent<IS_InventorySlot>();
            // Assign the id of these special slots
            classSlotArray[a].GetComponent<IS_InventorySlot>().slotID = a;
        }
    }

    private void DestroyInventory()
    {
        if (objectSlotList.Count >= 1)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                // Destroy the object associated with the inventory slot at this index
                Destroy(objectSlotList[i].gameObject);
                // Remove refrences to objects at this index
                classSlotArray[i] = null;
                objectSlotList[i] = null;
            }
            // Create a new object list after destorying the inventory
            objectSlotList = new List<GameObject>();
        }
        else
        {
            Debug.LogWarning("The inventory doesn't exist");
            return;
        }
    }
    
    private void SwapItems(int indexA, int indexB)
    {

        // Swap the item a and b on the back-end
        if (classSlotArray[indexB].slotType == ItemType.undefined || itemArray[indexA].itemType == classSlotArray[indexB].slotType 
            && itemArray[indexA] != null)
        {
            Debug.Log("Swap slot " + indexA + " and slot " + indexB);

            itemArray[indexA] = classSlotArray[indexB].item;
            // Update the item
            classSlotArray[indexA].UpdateItem(itemArray[indexA]);
        }
        else
        {
            // Reset the swap list
            swapSlotList = new List<IS_InventorySlot>();
            // Exit the function since the slot types are neither the same or undefined
            return;
        }

        // Swap item b and a on on the backend
        itemArray[indexB] = classSlotArray[indexA].item;
        // Update the item
        classSlotArray[indexB].UpdateItem(itemArray[indexB]);
        // Reset the swap list
        swapSlotList = new List<IS_InventorySlot>();
    }

    public void AddItemToInventory(SO_ItemBase item)
    {
        // Add item to the first empty slot
        for (int i = 0; i < inventorySize; i++)
        {
            // Add item to slot at this index if it has no item
            if (itemArray[i] == null) 
            {
                itemArray[i] = item;
                Debug.Log("Added " + item.name + " to inventory slot " + i);
                // Update the inventory slot at the slot ID i with the item at item array index i
                classSlotArray[i].UpdateItem(itemArray[i]);

                // Exit out of the function
                return; 
            }
            else Debug.LogWarning("Cannot add to inventory slot " + i);
        }
    }
}
