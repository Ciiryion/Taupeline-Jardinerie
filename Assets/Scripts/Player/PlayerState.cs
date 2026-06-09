using UnityEngine;

public class PlayerState : ObjectState<PlayerData>
{
    public Vector2 moveInput;
    public Vector2 mousePos;
    public float targetAngle;
    public bool isAttacking;
    public Weapon currentWeapon;
    public int nbrBullet;
    public float nextAttackTime;
    public bool isReloading;
}
