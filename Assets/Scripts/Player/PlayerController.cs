using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int life = 3;
    [SerializeField] private int walkSpeed = 5;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float tCrit = 5;
    [SerializeField] private float dCrit = 10;
    [SerializeField] private float defense = 5;
    [SerializeField] private Weapon currentWeapon;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private float targetAngle;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 lookDirection = (Vector2)mouseWorldPos - rb.position;
        targetAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        MovePlayer();
        rb.rotation = targetAngle;
    }

    void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);
    }

    void OnLook(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }
}
