using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public PhysicsControl physicsControl;
    public Animator anim;
    public StateMachine stateMachine;
    public PlayerStats playerStats;


    private BaseAbility[] playerAbilities;

    public bool facingRight = true;

    [Header("Current Weapon")]
    public GameObject currentWeaponPrefab;
    public ItemType currentWeaponType;

    [Header("Primary Weapon")]
    public GameObject primaryWeaponPrefab;

    [Header("Secondary Weapon")]
    public GameObject secondaryWeaponPos;
    public GameObject secondaryWeaponPrefab;

    [Header("Primary Weapon Positions")]
    [SerializeField] private Transform currentShootingPos;
    [SerializeField] private Transform crouchShootPos;
    [SerializeField] private Transform standingShootPos;
    [SerializeField] private Transform upShootPos;
    [HideInInspector] public Vector3 defaultWeaponVectorPos;

    [Header("Secondary Weapon Positions")]
    [SerializeField] private Transform secondCrouchShootPos;
    [SerializeField] private Transform secondStandingShootPos;
    [SerializeField] private Transform secondUpShootPos;


    public List<Weapon> listToSaveAndLoad = new List<Weapon>();



    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
        currentShootingPos = standingShootPos;
        defaultWeaponVectorPos = standingShootPos.localPosition;
    }

    private void OnDisable()
    {
        foreach (Weapon weapon in listToSaveAndLoad)
        {
            weapon.SaveWeaponData();
        }
    }

    private void Update()
    {
        foreach (BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Debug.Log("Current State is: " + stateMachine.currentState);
    }

    private void FixedUpdate()
    {
        foreach (BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }

        }
    }

    public void SetWeaponPosition()
    {
        if (stateMachine.currentState == PlayerStates.State.Crouch)
        {
            SetCrouchShootPos();
        }
        else if (stateMachine.currentState == PlayerStates.State.ShootUp)
        {
            SetUpShootPos();
        }
        else
        {
            SetStandShootPos();
        }
    }

    public void Flip()
    {
        if (facingRight == true && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
        else if (facingRight == false && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
    }

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    public void SetStandShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = standingShootPos;
            currentWeaponPrefab.transform.position = standingShootPos.position;

        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondStandingShootPos;
            currentWeaponPrefab.transform.position = secondStandingShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }
    public void SetCrouchShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = crouchShootPos;
            currentWeaponPrefab.transform.position = crouchShootPos.position;

        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondCrouchShootPos;
            currentWeaponPrefab.transform.position = secondCrouchShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }
    public void SetUpShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = upShootPos;
            currentWeaponPrefab.transform.position = upShootPos.position;

        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondUpShootPos;
            currentWeaponPrefab.transform.position = secondUpShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(90);
    }

    private void SetWeaponRotation(float zRotation)
    {
        currentWeaponPrefab.transform.localEulerAngles = new Vector3(0, 0, zRotation);
    }

    public void DeactivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(false);
    }
    public void ActivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(true);
    }

}
