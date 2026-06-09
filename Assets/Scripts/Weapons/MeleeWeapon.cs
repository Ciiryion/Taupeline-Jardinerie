using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/MeleeWeapon", fileName = "MeleeWeapon")]
public class MeleeWeapon : Weapon
{
    public float rangeW;
    private Collider2D[] hitColliders = new Collider2D[10]; // 10 Ennemis max touchés en même temps

    public override void ExecuteAttack(Transform attackPoint)
    {
        //Debug.Log("Attaque de melee");
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = enemyLayer;
        int numHits = Physics2D.OverlapCircle(attackPoint.position, rangeW, filter, hitColliders); // Détection du nombre d'ennemis dans la range
        for (int i = 0; i < numHits; i++)
        {
            Collider2D target = hitColliders[i];

            EnnemisBehaviour ennemis = target.GetComponent<EnnemisBehaviour>();
            if (ennemis != null)
            {
                float dmg = CalculDamage();
                ennemis.Hit(dmg, attackPoint.transform.right);
            }
        }
    }
}
