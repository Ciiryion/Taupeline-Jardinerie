using UnityEngine;

//[CreateAssetMenu(menuName = "Data/Weapon", fileName = "Weapon")]
public abstract class Weapon : ScriptableObject
{
    public float damageW;
    public float tCritW;
    public float dCritW;
    public float attackSpeedW;
    public LayerMask enemyLayer;
    public Sprite sprite;

    public abstract void ExecuteAttack(Transform attackPoint);

    protected float CalculDamage()
    {
        bool isCrit = Random.value < (tCritW / 100);
        //Debug.Log("isCrit = " +  isCrit);
        float damage = damageW;
        if (isCrit)
            damage += damage * (dCritW / 100);
        return damage;
    }
}
