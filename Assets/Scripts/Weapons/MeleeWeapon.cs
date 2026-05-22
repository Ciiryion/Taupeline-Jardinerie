using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/MeleeWeapon", fileName = "MeleeWeapon")]
public class MeleeWeapon : Weapon
{
    public float rangeW;

    public override void ExecuteAttack(Transform attackPoint)
    {
        Debug.Log("Attaque de melee");
    }
}
