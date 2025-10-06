using UnityEditor.Searcher;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string ID;
    public ItemType itemType;
    public float damage;
    public float shootCooldown;
    public bool isAutomatic;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public int storageAmmo;

    [Header("Reload")]
    public float reloadTime;
    public bool isReloading;

    [Header("Recoil")]
    public float recoilStrength;
    public float recoilTime;

    [Header("References")]
    public Transform shootingPoint;
    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public GameObject effectPrefab;
    public GameObject hitEffectPrefab;
    public Sprite weaponIconSprite;

    [Header("Audio")]
    public AudioSource source;

    [Header("LineRenderer")]
    public float widthMultiplier;
    public float visibleLineTime;

    [SerializeField]
    private WeaponData weaponData = new WeaponData();

    public bool ReloadCheck()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        if (neededAmmo <= 0 || storageAmmo <= 0)
            return false;

        return true;
    }

    public void Reload()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, storageAmmo);
        currentAmmo += ammoToReload;
        storageAmmo -= ammoToReload;
        isReloading = false;
    }

    public void SaveWeaponData()
    {
        weaponData.ID = ID;
        weaponData.currentAmmo = currentAmmo;
        weaponData.storageAmmo = storageAmmo;
        SaveLoadManager.instance.Save(weaponData, SaveLoadManager.instance.folderName, ID + ".json");
    }

    public void LoadWeaponData()
    {
        SaveLoadManager.instance.Load(weaponData, SaveLoadManager.instance.folderName, ID + ".json");
        if (weaponData.ID != "")
        {
            currentAmmo = weaponData.currentAmmo;
            storageAmmo = weaponData.storageAmmo;
        }
    }
}
