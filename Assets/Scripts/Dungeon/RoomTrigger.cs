using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Collider2D _roomCollider;

    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] private EnnemisBehaviour prefMob;

    void Start()
    {
        _roomCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //PlayerController player = other.GetComponent<PlayerController>();
        PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            RoomManager.instance?.EnterRoom(_roomCollider);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //PlayerController player = other.GetComponent<PlayerController>();
        PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            RoomManager.instance?.ExitRoom(_roomCollider);
        }
    }

    public void SpawnMobs(int nbrMobs)
    {
        //Debug.Log("SpawnMobs");

        // Sécurité
        int actualSpawnCount = Mathf.Min(nbrMobs, spawnPointList.Count);

        List<Transform> avalaiblePoints = new List<Transform>(spawnPointList);

        for(int i = 0; i < actualSpawnCount; i++)
        {
            int rndIdx = Random.Range(0, avalaiblePoints.Count);
            Transform selectedPoint = avalaiblePoints[rndIdx];

            Instantiate(prefMob, selectedPoint.position, Quaternion.identity);

            avalaiblePoints.RemoveAt(rndIdx);
        }
    }
}