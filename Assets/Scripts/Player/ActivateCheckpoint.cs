using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckpoint : MonoBehaviour
{
    public InputActionReference ActivateCheck;
    [HideInInspector]
    public Checkpoint checkPoint;

    private void OnEnable()
    {
        ActivateCheck.action.Enable();
        ActivateCheck.action.performed += TryToActivate;
    }

    private void OnDisable()
    {
        ActivateCheck.action.Disable();
        ActivateCheck.action.performed -= TryToActivate;
        
    }
    private void TryToActivate(InputAction.CallbackContext value)
    {
        if (checkPoint == null)
            return;
        //activate
        checkPoint.Activate();
    }
}
