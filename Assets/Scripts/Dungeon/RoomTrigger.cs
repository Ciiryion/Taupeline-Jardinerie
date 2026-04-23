using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Collider2D _roomCollider;

    void Start()
    {
        _roomCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager.instance.EnterRoom(_roomCollider);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && RoomManager.instance != null)
        {
            RoomManager.instance.ExitRoom(_roomCollider);
        }
    }
}