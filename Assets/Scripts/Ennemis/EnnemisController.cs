using UnityEngine;

public class EnnemisController : MonoBehaviour
{
    [SerializeField] private int life = 3;
    [SerializeField] private int walkSpeed = 5;
    [SerializeField] private float attackSpeed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hit()
    {
        life--;
        Debug.Log("life = " + life);
        if (life <= 0)
            IsDead();
    }

    private void IsDead()
    {
        Destroy(gameObject);
        return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hit();
    }

}
