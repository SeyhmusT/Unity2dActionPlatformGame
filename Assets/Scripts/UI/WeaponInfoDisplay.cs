using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponInfoDisplay : MonoBehaviour
{
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private TextMeshProUGUI currentAmmoTxt;
    [SerializeField] private TextMeshProUGUI storageAmmoTxt;

    private void OnEnable()
    {
        Shooting.OnUpdateAllInfo += UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo += UpdateAmmoInfo; 
    }
    private void OnDisable()
    {
        Shooting.OnUpdateAllInfo -= UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo -= UpdateAmmoInfo; 
    }

    private void UpdateAllWeaponInfo(Sprite weaponSpriteIcon, int currentAmmo, int maxAmmo, int storageAmmo)
    {
        currentWeaponIcon.sprite = weaponSpriteIcon;
        UpdateAmmoInfo(currentAmmo, maxAmmo, storageAmmo);
    }


    private void UpdateAmmoInfo(int currentAmmo, int maxAmmo, int storageAmmo)
    {
        currentAmmoTxt.text = currentAmmo.ToString() + "<i>l<i>" + maxAmmo.ToString();
        storageAmmoTxt.text = storageAmmo.ToString();
    }
}  
