using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Data/Inventory System/XXX", fileName = "YYY")]
public class SO_ItemBase : ScriptableObject
{
    [Header("Base Parameters")]
    public ItemType itemType;
    [Space(10)]

    public string itemName = "Base Item";
    [TextArea(5, 10)] public string itemDescritption = "This is a base item.";
    [Space(10)]

    public Sprite itemSprite;
}
