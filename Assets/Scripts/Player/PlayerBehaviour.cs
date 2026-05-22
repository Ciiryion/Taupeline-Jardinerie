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
