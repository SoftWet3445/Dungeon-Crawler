using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private float interactionRange = 1.0f;
    [Space(10)]

    [SerializeField] private Camera interactionCamera;

    private void Start()
    {
        interactionCamera = this.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { TryInteractTouch(); }
    }

    private void TryInteractTouch()
    {
        //if (MouseOverUI()) { return; }

        //TODO: change input.mouseposition to work with new input system
        Ray ray = interactionCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, interactionRange))
        {
            // Get the interactable that the raycast hits
            IInteractable interactable = hitinfo.collider.GetComponent<IInteractable>();
            // Interact with th eobject if it exists
            if (interactable != null) { interactable.Interact(); }
            Debug.Log("interact with " + hitinfo.transform.gameObject.name);
        }
    }

    //TODO: migrate this into a game manager/handler of some sort
    private bool MouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return true; }
        else { return false; }
    }
}
