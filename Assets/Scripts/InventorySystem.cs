using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { weapon, shield, ammo, trinket, consumable, head, body, legs, undefined }
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    public int inventorySize = 15;
    public int specialSlots = 9;
    [Space(10)]

    [SerializeField] private GameObject inventorySlotPrefab;
    [Space(10)]

    [SerializeField] private Transform slotHolder;
    [Space(10)]

    public ObjectItemBase[] itemArray;
    [SerializeField] private InventorySlot[] classSlotArray;
    [SerializeField] private List<GameObject> objectSlotList;
    [Space(10)]
    //[SerializeField] private ObjectItemBase testAddItem;

    public List<InventorySlot> swapSlotList;

    private Image itemIconMouseFollower;

    private void Awake() => instance = this;

    private void Start()
    {
        // Instantiate arrays
        classSlotArray = new InventorySlot[inventorySize + specialSlots];
        itemArray = new ObjectItemBase[inventorySize + specialSlots];
        // Instantiate lists
        objectSlotList = new List<GameObject>();
        swapSlotList = new List<InventorySlot>();

        itemIconMouseFollower = transform.GetChild(3).GetComponent<Image>();

        // Create the inventory
        DisplayInventory();
        // Add the special slots to the inventory
        AddSpecialSlots();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild) { AddItemToInventory(testAddItem); }
        //if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild) { DestroyInventory(); }
        //if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild) { DisplayInventory(); }

        if (swapSlotList.Count == 2) 
        {
            SwapItems(swapSlotList[0].slotID, swapSlotList[1].slotID);
        }

        if (swapSlotList.Count > 0) 
        { 
            itemIconMouseFollower.enabled = true; 
            itemIconMouseFollower.sprite = swapSlotList[0].item.itemSprite;
            // Disable the icon of the item thats being held
            classSlotArray[swapSlotList[0].slotID].itemIcon.enabled = false;
        }
        else 
        {
            itemIconMouseFollower.enabled = false;
            //classSlotArray[swapSlotList[0].slotID].itemIcon.enabled = true;
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
            newSlot = Instantiate(inventorySlotPrefab, transform.position, Quaternion.identity, slotHolder);
            // Add the inventory slot's class to the inventory slot array
            classSlotArray[i] = newSlot.GetComponent<InventorySlot>();
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
        for (int i = 0; i < 9; i++)
        {
            // Create index a, being i + the invetory size minus one (arrays are zero indexed) to account for bulk slots
            int a = i + (inventorySize);
            // Add the special inventory slot at index a 
            classSlotArray[a] = transform/*.parent*/.GetChild(1).GetChild(i).GetComponent<InventorySlot>();
            // Assign the id of these special slots
            classSlotArray[a].GetComponent<InventorySlot>().slotID = a;
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
            classSlotArray[swapSlotList[0].slotID].itemIcon.enabled = true;
            // Reset the swap list
            swapSlotList = new List<InventorySlot>();
            // Exit the function since the slot types are neither the same or undefined
            return;
        }

        // Swap item b and a on on the backend
        itemArray[indexB] = classSlotArray[indexA].item;
        // Update the item
        classSlotArray[indexB].UpdateItem(itemArray[indexB]);

        classSlotArray[swapSlotList[0].slotID].itemIcon.enabled = true;
        // Reset the swap list
        swapSlotList = new List<InventorySlot>();
    }

    public void AddItemToInventory(ObjectItemBase item)
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
