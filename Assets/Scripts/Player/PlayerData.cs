using UnityEngine;

[CreateAssetMenu(menuName = "Entity/PlayerData")]
public class PlayerData : ObjectData
{
    public int walkSpeed = 1;
    public Weapon startingWeapon;
}
