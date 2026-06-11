using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    public Weapon weaponData;

    public void Initialize(Weapon newWeapon)
    {
        weaponData = newWeapon;
        if (weaponData != null)
        {
            GetComponent<SpriteRenderer>().sprite = weaponData.sprite;
        }
    }
}
