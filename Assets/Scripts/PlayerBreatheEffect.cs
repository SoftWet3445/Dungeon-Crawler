using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreatheEffect : MonoBehaviour
{
    [SerializeField] private float idleCounter;
    //[SerializeField] private float movementCounter;
    [Space(10)]

    [SerializeField] private Transform weaponParent;
    [Space(10)]

    [SerializeField] private Vector3 targetWeaponParentPosition;
    [SerializeField] private Vector3 weaponParentOrigin;

    private void Start()
    {
        weaponParentOrigin = weaponParent.localPosition;
    }

    private void Update()
    {
        //if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
        if (true)
        {
            // Breathe with idle parameters
            WeaponBreathing(idleCounter, 0.0325f, 0.0325f);
            // Increase the idle counter if the player is not moving
            idleCounter += Time.deltaTime;
        }
        /* UNUSED faster breathing implementation
        else
        {
            // Breathe with movement parameters 
            WeaponBreathing(movementCounter, 0.06f, 0.06f);
            // Increase the movement counter when player is moving
            movementCounter += Time.deltaTime * 3.5f;
        }
        */
        // Lerp towards the wanted weapon position position
        weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponParentPosition, Time.deltaTime * 8.0f);
        // Reset the idle counter if its greater than the max 32-bit integer value, 2 147 483 647
        if ((int)idleCounter >= int.MaxValue) { idleCounter = 0.0f; }
    }

    private void WeaponBreathing(float z, float xItensity, float yIntensity)
    {
        // Move in a figure eight motion to create the illusion of breathing
        targetWeaponParentPosition = weaponParentOrigin + new Vector3(Mathf.Cos(z) * xItensity, Mathf.Sin(z * 2.0f) * yIntensity, 0.0f);
    }
}
