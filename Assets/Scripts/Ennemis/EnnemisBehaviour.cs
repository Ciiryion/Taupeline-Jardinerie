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
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Hit()
    {
        State.life--;
        if (State.life <= 0)
            IsDead();
    }

    private void IsDead()
    {
        Destroy(gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void InstanceCreated()
    { }
}
