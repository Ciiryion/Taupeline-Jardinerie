using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/BulletData")]
public class BulletData : ObjectData
{
    public int damage = 1;
    public float lifetime = 3f;
    public float speed = 20f;
}
