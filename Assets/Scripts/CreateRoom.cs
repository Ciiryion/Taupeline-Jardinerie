using UnityEngine;

public class CreateRoom : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private Transform roomPosition;

    private void Start()
    {
        Instantiate(room, roomPosition.position, roomPosition.rotation, transform); 
    }
}
