using UnityEngine;

[CreateAssetMenu(menuName = "Entity/EnnemisData")]
public class EnnemisData : ObjectData
{
    public int walkSpeed = 1;
    public int maxLife = 5;
    public Weapon Weapon;
}
