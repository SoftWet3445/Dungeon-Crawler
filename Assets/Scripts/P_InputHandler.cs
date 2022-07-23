using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(P_WeaponHandler))]
[RequireComponent(typeof(G_GridMovement))]
public class P_InputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode forward = KeyCode.W;
    [SerializeField] private KeyCode backward = KeyCode.S;
    [SerializeField] private KeyCode leftward = KeyCode.A;
    [SerializeField] private KeyCode rightward = KeyCode.D;
    [Space(10)]
    
    [SerializeField] private KeyCode lookLeft = KeyCode.Q;
    [SerializeField] private KeyCode lookRight = KeyCode.E;
    [Space(10)]

    [SerializeField] private KeyCode attack = KeyCode.Space;

    private G_GridMovement gridMovement;
    private P_WeaponHandler weaponHandler;
    
    private void Start()
    {
        gridMovement = this.GetComponent<G_GridMovement>();
        weaponHandler = this.GetComponent<P_WeaponHandler>();
    }

    private void Update()
    {
        HandleMovement();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleMovement()
    {
        // 4-directional movement input
        if (Input.GetKeyDown(forward)) { gridMovement.MoveForward(); }
        if (Input.GetKeyDown(backward)) { gridMovement.MoveBackward();}
        if (Input.GetKeyDown(leftward)) { gridMovement.MoveLeftward(); }
        if (Input.GetKeyDown(rightward)) { gridMovement.MoveRightward(); }
        
        // Left and right camera look input
        if (Input.GetKeyDown(lookLeft)) { gridMovement.RotateLeft(); }
        if (Input.GetKeyDown(lookRight)) { gridMovement.RotateRight(); }

        // Attack input
        if (Input.GetKeyDown(attack)) { weaponHandler.TryUseWeapon(); }
    }
}
