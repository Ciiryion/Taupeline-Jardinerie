using UnityEngine;
using UnityEngine.InputSystem;

public class EnnemisBehaviour : ObjectBehaviour<EnnemisInstance, EnnemisState, EnnemisData>
{
    private Rigidbody2D rb;
    [SerializeField] private Transform attackPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        State.life = Data.maxLife;

        State.pathfinding = FindFirstObjectByType<Pathfinding>();

        GameManager.RegisterEnemy();
    }

    private void Update()
    {
        if(State.knockbackTimer > 0)
            State.knockbackTimer -= Time.deltaTime;

        CheckDetection();

        if(State.isAggro && GameManager.player != null)
        {
            State.pathUpdateTimer += Time.deltaTime;
            if(State.pathUpdateTimer >= Data.pathUpdateInterval)
            {
                State.pathUpdateTimer = 0;
                State.currentPath = State.pathfinding.FindPath(transform.position, GameManager.player.transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if (State.knockbackTimer > 0) return;

        if(State.isAggro && State.currentPath != null && State.currentPath.Count > 0)
        {
            Vector3 targetTilePos = State.currentPath[0].worldPosition;

            if(Vector3.Distance(transform.position, targetTilePos) < 0.2f)
            {
                State.currentPath.RemoveAt(0);
                return;
            }

            Vector2 direction = ((Vector2)targetTilePos - rb.position).normalized;

            rb.linearVelocity = direction * Data.walkSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void CheckDetection()
    {
        if (GameManager.player == null) return;

        float currentRadius = State.isAggro ? Data.aggroRadius : Data.detectionRadius;
        float distanceToPlayerSqr = (GameManager.player.transform.position - transform.position).sqrMagnitude;

        if (distanceToPlayerSqr <= (currentRadius * currentRadius))
        {
            Vector2 directionToPlayer = (GameManager.player.transform.position - transform.position).normalized;
            float distanceToPlayer = Mathf.Sqrt(distanceToPlayerSqr);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, Data.obstacleLayer);

            if(hit.collider == null)
                State.isAggro = true;
        }
        else
        {
            State.isAggro = false;
            State.currentPath = null;
        }
    }

    public void Hit(float damage, Vector2 attackDirection)
    {
        //Debug.Log($"Attaque de {damage} dégâts");
        //Debug.Log($"Il reste {State.life} vie");
        State.life -= damage;
        if (State.life <= 0)
        {
            IsDead();
            return;
        }
        rb.AddForce(attackDirection * Data.kbForce, ForceMode2D.Impulse); // Knockback
        State.knockbackTimer = 0.2f;
    }

    private void IsDead()
    {
        GameManager.UnregisterEnemy();

        Destroy(gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void InstanceCreated()
    { }
}
