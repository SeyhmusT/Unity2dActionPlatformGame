using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class ReloadAbility : BaseAbility
{
    public InputActionReference reloadActionRef;
    [SerializeField] private ReloadBar reloadBar;
    private Weapon currentWeapon;
    private Coroutine reloadCoroutine;

    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    }

    public override void EnterAbility()
    {
        
        linkedPhysics.ResetVelocity();
    }

    private void TryToReload(InputAction.CallbackContext value)
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();

        if (!isPermitted || player.currentWeaponPrefab == null)
            return;


        if (linkedPhysics.grounded == false || linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash
            || linkedStateMachine.currentState == PlayerStates.State.Knockback)
            return;

        if (currentWeapon.ReloadCheck() == false || currentWeapon.isReloading)
            return;

        reloadCoroutine = StartCoroutine(ReloadProcess());
        source.PlayOneShot(clip);
}


    private void OnEnable()
    {
        if (reloadActionRef != null && reloadActionRef.action != null)
            reloadActionRef.action.Enable();

        reloadActionRef.action.performed += TryToReload;
    }

    private void OnDisable()
    {
        if (reloadActionRef != null && reloadActionRef.action != null)
            reloadActionRef.action.Disable();

        reloadActionRef.action.performed -= TryToReload;

    }

    private IEnumerator ReloadProcess()
    {
        linkedStateMachine.ChangeState(PlayerStates.State.Reload);
        currentWeapon.isReloading = true;
        reloadBar.ActivateReloadBar();
        float elapsedTime = 0;
        while (elapsedTime < currentWeapon.reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadBar.UpdateReloadBar(elapsedTime, currentWeapon.reloadTime);
            yield return null;
        }
        reloadBar.DeactivateReloadBar();
        currentWeapon.Reload();

        Shooting.OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

        if (linkedStateMachine.currentState != PlayerStates.State.Death && linkedStateMachine.currentState != PlayerStates.State.Knockback)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
    }

    public override void ExitAbility()
    {
        reloadBar.DeactivateReloadBar();
        if (reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);

        currentWeapon.isReloading = false;
    }

    public override void UpdateAnimator()
    {
        //update stuff
    }
}
