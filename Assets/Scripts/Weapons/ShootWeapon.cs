using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/ShootWeapon", fileName = "ShootWeapon")]
public class ShootWeapon : Weapon
{
    public int nbrBullet;
    public float reloadTime;
    public GameObject bulletPrefab;

    public override void ExecuteAttack(Transform attackPoint)
    {
        if(GameManager.player.State.nbrBullet > 0 && !GameManager.player.State.isReloading)
        {
            Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
            GameManager.player.State.nbrBullet--;
        }
        else
        {
            GameManager.player.reload();
        }
    }

}
