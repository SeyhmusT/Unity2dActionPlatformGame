using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionReference interactActionRef;
    private IInteractable currentInteractable;

    private void TryToInteract(InputAction.CallbackContext value)
    {
        if (currentInteractable != null)
        {
            currentInteractable.CustomInteract();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = null;
        }
    }

    void OnEnable()
    {
        if (interactActionRef != null && interactActionRef.action != null)
            interactActionRef.action.Enable();

        interactActionRef.action.performed += TryToInteract;
    }

    void OnDisable()
    {
        if (interactActionRef != null && interactActionRef.action != null)
            interactActionRef.action.Disable();

        interactActionRef.action.performed -= TryToInteract;
    }
}
