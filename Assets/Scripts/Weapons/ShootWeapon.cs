using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/ShootWeapon", fileName = "ShootWeapon")]
public class ShootWeapon : Weapon
{
    public int nbrBullet;
    public float reloadTime;
    public GameObject bulletPrefab;

    public override void ExecuteAttack(Transform attackPoint)
    {
        Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
    }
}
