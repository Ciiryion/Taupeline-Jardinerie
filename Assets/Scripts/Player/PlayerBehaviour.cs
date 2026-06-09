using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : ObjectBehaviour<PlayerInstance, PlayerState, PlayerData>
{
    private Rigidbody2D rb;
    [SerializeField] private Transform attackPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        State.currentWeapon = Data.startingWeapon;
        reload();
    }

    private void Update()
    {
        Vector3 mouseWorldPos = GameManager.mainCamera.ScreenToWorldPoint(State.mousePos);
        Vector2 lookDirection = (Vector2)mouseWorldPos - rb.position;
        State.targetAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (State.isAttacking && Time.time >= State.nextAttackTime && State.currentWeapon != null)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        rb.rotation = State.targetAngle;
    }

    void MovePlayer()
    {
        rb.linearVelocity = new Vector2(State.moveInput.x * Data.walkSpeed, State.moveInput.y * Data.walkSpeed);
    }

    
    private void Attack()
    {
        State.currentWeapon.ExecuteAttack(attackPoint);
        State.nextAttackTime = Time.time + (1f / State.currentWeapon.attackSpeedW);
    }


    protected override void InstanceCreated()
    {
        GameManager.player = this;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null || State.currentWeapon == null) return;


        if (State.currentWeapon is MeleeWeapon meleeWeapon)
        {
            // On dessine la zone d'attaque uniquement pour les armes de mêlée
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, meleeWeapon.rangeW);
        }
    }

    public void reload()
    {
        State.isReloading = true;
        StartCoroutine(nameof(reloading));
        
    }

    private IEnumerator reloading()
    {
        if (State.currentWeapon is ShootWeapon shootWeapon)
        {
            State.nbrBullet = shootWeapon.nbrBullet;
            yield return new WaitForSeconds(shootWeapon.reloadTime);
            State.isReloading = false;
        }
    }


    //*************************************************************************************//
    //********************************* METHODES D'INPUT **********************************//
    //*************************************************************************************//
    void OnMovement(InputValue value)
    {
        State.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        State.mousePos = value.Get<Vector2>();
    }

    void OnAttack(InputValue value)
    {
        State.isAttacking = value.isPressed;
    }
}
