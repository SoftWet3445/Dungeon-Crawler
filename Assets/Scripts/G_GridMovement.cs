using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GridMovement : MonoBehaviour
{
    [SerializeField] private bool smoothTransition = true;
    [Space(10)]
    
    [SerializeField] private float transitionSpeed = 10.0f;
    [SerializeField] private float transitionRotationSpeed = 500.0f;
    [SerializeField] private float collisionCheckDistance = 1.0f;
    [Space(10)]

    [SerializeField] private LayerMask obstructionMask;

    private bool canMove;
    private bool canRotate;

    private float moveCooldown = 0.15f;
    private float rotateCooldown = 0.15f;

    private Vector3 targetGridPos;
    private Vector3 targetRotation;

    private void Start()
    {
        targetGridPos = transform.position;
        canMove = true;
        canRotate = true;
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (true)
        {
            Vector3 targetPosition = targetGridPos;

            // Reset the target rotation if it meets a certain threshold
            if (targetRotation.y > 270.0f && targetRotation.y < 361.0f) targetRotation.y = 0.0f;
            if (targetRotation.y < 0.0f) targetRotation.y = 270.0f;

            if (!smoothTransition)
            {
                // Change position and rotation directly
                transform.position = targetPosition;
                transform.rotation = Quaternion.Euler(targetRotation);
            }
            else
            {
                // Change position and rotation smoothly
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            }
        }
    }

    public void RotateLeft()
    {
        if (!canRotate) { return; }
        // Update the target rotation if at rest
        if (AtRest) { targetRotation -= Vector3.up * 90f; canRotate = false; StartCoroutine(ResetRotationBool()); }
    }

    public void RotateRight()
    {
        if (!canRotate) { return; }
        // Update the target rotation if at rest
        if (AtRest) { targetRotation += Vector3.up * 90f; canRotate = false; StartCoroutine(ResetRotationBool()); }
    } 

    public void MoveForward()
    {
        if (!canMove) { return; }
        // Only move if the player is at rest and no object is in front
        if (AtRest && TryMove(transform.forward)) { targetGridPos += transform.forward; canMove = false; StartCoroutine(ResetMoveBool()); }
    }

    public void MoveBackward()
    {
        if (!canMove) { return; }
        // Only move if the player is at rest and no object is behind
        if (AtRest && TryMove(-transform.forward)) { targetGridPos -= transform.forward; canMove = false; StartCoroutine(ResetMoveBool()); }
    }

    public void MoveLeftward()
    {
        if (!canMove) { return; }
        // Only move if the player is at rest and no object is to the left
        if (AtRest && TryMove(-transform.right)) { targetGridPos -= transform.right; canMove = false; StartCoroutine(ResetMoveBool()); }
    }

    public void MoveRightward()
    {
        if (!canMove) { return; }
        // Only move if the player is at rest and no object is to the right
        if (AtRest && TryMove((transform.right))) { targetGridPos += transform.right; canMove = false; StartCoroutine(ResetMoveBool()); }
    }

    private bool TryMove(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out var hit, collisionCheckDistance, obstructionMask))
        {
            // Return false if an object is in the way
            if (!hit.transform.gameObject.GetComponent<Collider>().isTrigger) { return false; }
        }
        // Return true if an object is not in the way
        return true;
    }
    
    public bool AtRest
    {
        get
        {
            // If the distance to the target position and target rotation is zero, not moving 
            if ((Vector3.Distance(transform.position, targetGridPos) == 0)
                && Vector3.Distance(transform.eulerAngles, targetRotation) == 0) { return true; }          
            else { return false; }
        }
    }

    private IEnumerator ResetMoveBool()
    {
        yield return new WaitForSeconds(moveCooldown);
        canMove = true;
    }

    private IEnumerator ResetRotationBool()
    {
        yield return new WaitForSeconds(rotateCooldown);
        canRotate = true;
    }

    public void CannotMove() { canMove = false; canRotate = false; }
    public void CanMove() { canMove = true; canRotate = true; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        // Draw a gizmos ray in the normal direction of this object
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward * collisionCheckDistance);
    }
}
