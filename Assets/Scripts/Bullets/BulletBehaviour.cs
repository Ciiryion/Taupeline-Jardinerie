using UnityEngine;
using UnityEngine.InputSystem;

public class BulletBehaviour : ObjectBehaviour<BulletInstance, BulletState, BulletData>
{
    private Rigidbody2D rb;

    protected override void InstanceCreated()
    { }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, Data.lifetime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.right.normalized * Data.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }

        EnnemisBehaviour ennemi = collision.GetComponent<EnnemisBehaviour>();
        if (ennemi != null)
        {
            Destroy(gameObject);
            ennemi.Hit(Data.damage, transform.right);
        }


    }
}
