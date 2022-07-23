using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory System/Weapon Item", fileName = "newWeaponItem")]
public class SO_WeaponItem : SO_ItemBase
{
    [Header("Weapon Parameters")]
    public float damage = 10;
    public float range = 1;
    [Space(10)]

    //[Tooltip("Sprite that appears as the in-game weapon")] public Sprite heldSprite;
    [Tooltip("Sprite that appears on the inventory player")] public Sprite equippedSprite;

    private void Awake()
    {
        itemType = ItemType.weapon;
    }
}
