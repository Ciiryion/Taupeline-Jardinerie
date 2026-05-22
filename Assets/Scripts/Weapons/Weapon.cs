using UnityEngine;

//[CreateAssetMenu(menuName = "Data/Weapon", fileName = "Weapon")]
public abstract class Weapon : ScriptableObject
{
    public float damageW;
    public float tCritW;
    public float dCritW;
    public float attackSpeedW;

    public abstract void ExecuteAttack(Transform attackPoint);

    protected float CalculDamage()
    {
        bool isCrit = Random.value < (tCritW / 100);
        float damage = damageW;
        if (isCrit)
            damage *= (dCritW / 100);
        return damage;
    }
}
