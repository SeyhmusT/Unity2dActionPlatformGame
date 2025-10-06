using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public InputActionReference shootActionRef;
    public InputActionReference changeWeaponRef;
    public Weapon currentWeapon;
    private Player player;
    private ItemType currentWeaponType;
    private bool shootButtonHeld;
    private bool shootCooldownOver = true;
    private PlayerInput playerInput;
    

    [Header("Raycast Stuff")]
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private LineRenderer lineRender;
    private bool isShootLineActive = false;
    private Vector3 startPoint;
    private Vector3 endPoint;


    public static Action<Sprite, int, int, int> OnUpdateAllInfo;
    public static Action<int, int, int> OnUpdateAmmo;
    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = player != null ? player.GetComponent<PlayerInput>() : null;
    }
    void Start()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        LoadWeapons();
        // UI'ı güncellemeden önce kısa bir gecikme ekleyelim
        StartCoroutine(InitialUIUpdate());
    }
    

    private IEnumerator InitialUIUpdate()
    {
        // Tüm verilerin yüklenmesi için bir frame bekleyelim
        yield return new WaitForEndOfFrame();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentWeapon != null)
        {
            OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
        }
    }
    private void OnEnable()
    {
        if (shootActionRef != null && shootActionRef.action != null)
            shootActionRef.action.Enable();

        if (changeWeaponRef != null && changeWeaponRef.action != null)
            changeWeaponRef.action.Enable();

        changeWeaponRef.action.performed += TryToChangeWeapon;
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShooting;
    }

    private void OnDisable()
    {
        if (shootActionRef != null && shootActionRef.action != null)
            shootActionRef.action.Disable();

        if (changeWeaponRef != null && changeWeaponRef.action != null)
            changeWeaponRef.action.Disable();

        changeWeaponRef.action.performed -= TryToChangeWeapon;
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShooting;
    }

    private void TryToShoot(InputAction.CallbackContext value)
    {
        if (playerInput != null && playerInput.currentActionMap != null && playerInput.currentActionMap.name != "Player" || currentWeapon == null || player.stateMachine.currentState == PlayerStates.State.Ladders || player.stateMachine.currentState == PlayerStates.State.Dash ||
            player.stateMachine.currentState == PlayerStates.State.WallSlide || player.stateMachine.currentState == PlayerStates.State.Knockback)
            return;

        if (shootButtonHeld || shootCooldownOver == false)
            return;

        if (currentWeapon.isAutomatic)
        {
            shootButtonHeld = true;
            return;
        }
        shootButtonHeld = true;

        Shoot();
        
    }

    private void TryToChangeWeapon(InputAction.CallbackContext value)
    {
        if (playerInput != null && playerInput.currentActionMap != null && playerInput.currentActionMap.name != "Player" || currentWeapon == null || player.stateMachine.currentState == PlayerStates.State.Ladders || player.stateMachine.currentState == PlayerStates.State.Dash ||
        player.stateMachine.currentState == PlayerStates.State.WallSlide || player.stateMachine.currentState == PlayerStates.State.Knockback)
            return;

        if (currentWeapon.isReloading)
            return;

        if (currentWeapon.itemType == ItemType.PrimaryWeapon)
        {
            if (player.secondaryWeaponPrefab == null)
                return;

            player.primaryWeaponPrefab.SetActive(false);
            player.secondaryWeaponPrefab.SetActive(true);
            player.currentWeaponPrefab = player.secondaryWeaponPrefab;
            currentWeaponType = ItemType.SecondaryWeapon;
            player.currentWeaponType = currentWeaponType;
            currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
            player.anim.SetLayerWeight(1, 1);
            player.SetWeaponPosition();
        }
        else
        {
            if (player.primaryWeaponPrefab == null)
                return;

            player.secondaryWeaponPrefab.SetActive(false);
            player.primaryWeaponPrefab.SetActive(true);
            player.currentWeaponPrefab = player.primaryWeaponPrefab;
            currentWeaponType = ItemType.PrimaryWeapon;
            player.currentWeaponType = currentWeaponType;
            currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
            player.anim.SetLayerWeight(1, 0);
            player.SetWeaponPosition();
        }
        UpdateUI();
    }

    private void StopShooting(InputAction.CallbackContext value)
    {
        shootButtonHeld = false;
    }

    public void LoadWeapons()
    {
        foreach (Weapon weapon in player.listToSaveAndLoad)
        {
            weapon.LoadWeaponData();
        }
    }

    private void Shoot()
    {
        if (currentWeapon.currentAmmo <= 0 || currentWeapon.isReloading)
            return;

        currentWeapon.source.Play();

        Instantiate(currentWeapon.shellPrefab, currentWeapon.shellSpawnPoint.position, currentWeapon.transform.rotation);
        currentWeapon.effectPrefab.transform.position = currentWeapon.shootingPoint.position;
        currentWeapon.effectPrefab.SetActive(true);

        if (player.stateMachine.currentState != PlayerStates.State.ShootUp)
            currentWeapon.transform.localPosition = player.defaultWeaponVectorPos - Vector3.right * currentWeapon.recoilStrength;
        else
            currentWeapon.transform.localPosition = player.defaultWeaponVectorPos - Vector3.up * currentWeapon.recoilStrength;

        
        lineRender.positionCount = 2;
        lineRender.widthMultiplier = currentWeapon.widthMultiplier;
        Vector3 direction = currentWeapon.shootingPoint.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoint.position, direction, Mathf.Infinity, whatToHit);
        if (hitInfo)
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = hitInfo.point;

            Vector2 normal = hitInfo.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(currentWeapon.hitEffectPrefab, hitInfo.point, rotation);

            
            EnemyStats enemyStats = hitInfo.collider.GetComponent<EnemyStats>();
            IDamageable damageableObject = hitInfo.collider.GetComponent<IDamageable>();

            if (enemyStats != null)
            {
                // for enemies
                enemyStats.TakeDamage(currentWeapon.damage);
            }
            else if (damageableObject != null)
            {
                //destructible objects
                damageableObject.TakeDamage(currentWeapon.damage);
            }
            Debug.Log("We Hit Something");
        }
        else
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = currentWeapon.shootingPoint.position + direction * 10;

            Debug.Log("We Hit Nothing");
        }
        currentWeapon.currentAmmo -= 1;
        
        StartCoroutine(ShootDelay());
        StartCoroutine(ResetShootLine());
        OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    public void AddStorageAmmo(String ID, int AmmoToAdd)
    {
        foreach (Weapon weapon in player.listToSaveAndLoad)
        {
            if (weapon.ID == ID)
            {
                weapon.storageAmmo += AmmoToAdd;
                OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
                break;
            }
        }
    }

    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.recoilTime);
        currentWeapon.transform.localPosition = player.defaultWeaponVectorPos;
        yield return new WaitForSeconds(currentWeapon.shootCooldown - currentWeapon.recoilTime);
        shootCooldownOver = true;
    }
    private IEnumerator ResetShootLine()
    {
        isShootLineActive = true;
        
        yield return new WaitForSeconds(currentWeapon.visibleLineTime);
        lineRender.positionCount = 0;
        isShootLineActive = false;
    }

    void Update()
    {
    if (playerInput != null && playerInput.currentActionMap != null && playerInput.currentActionMap.name != "Player")
    {
        if (shootButtonHeld && currentWeapon.isAutomatic && shootCooldownOver)
            Shoot();
    }

    if (isShootLineActive)
    {
        lineRender.SetPosition(0, currentWeapon.shootingPoint.position);
        lineRender.SetPosition(1, endPoint);
    }
    }
}
