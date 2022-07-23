using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_WeaponHandler : MonoBehaviour
{
    public SO_WeaponItem currentWeapon;
    [Space(10)]

    [SerializeField] private float weaponDamage = 0.0f;
    [SerializeField] private float weaponRange = 1.0f;
    [Space(10)]

    private bool canUseWeapon;

    private SpriteRenderer weaponSR;
    private Animator weaponAnimator;

    private Transform weaponTransform;
    private Transform weaponPivot;

    private void Start()
    {
        weaponTransform = GameObject.FindGameObjectWithTag("PlayerWeaponObject").transform;
        weaponSR = weaponTransform.GetComponent<SpriteRenderer>();

        weaponPivot = weaponTransform.parent;

        weaponAnimator = weaponPivot.GetComponent<Animator>();  

        // Update held weapon at the game start if it exists
        if (currentWeapon != null) { UpdateHeldWeapon(); }

        canUseWeapon = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Debug.isDebugBuild) { UpdateHeldWeapon(); }
    }

    private void UpdateHeldWeapon()
    {
        // Update the weapon's sprite
        weaponSR.sprite = currentWeapon.itemSprite;

        // Update the weapon's values
        weaponDamage = currentWeapon.damage;
        weaponRange = currentWeapon.range;
    }

    public void TryUseWeapon()
    {
        // Don't use weapon if can't use weapon
        if (!canUseWeapon) { return; }

        // Use the weapon
        StartCoroutine(UseWeapon());
    }

    private IEnumerator UseWeapon()
    {
        //if (!canUseWeapon) { yield break; }

        canUseWeapon = false;

        gameObject.SendMessage("CannotMove");

        // Play the attack animation
        weaponAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(0.5f );

        // Reset the attack trigger
        weaponAnimator.ResetTrigger("attack");
        // Reset the animation speed
        weaponAnimator.speed = 1.0f;

        gameObject.SendMessage("CanMove");

        canUseWeapon = true;
    }
}
