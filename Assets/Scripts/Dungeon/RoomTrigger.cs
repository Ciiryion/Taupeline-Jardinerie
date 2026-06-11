using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RoomTrigger : MonoBehaviour
{
    private Collider2D _roomCollider;

    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] private List<EnnemisBehaviour> prefMob;

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
            int ennemyType = Random.Range(0, 101);
            switch (ennemyType)
            {
                case var _ when ennemyType < 70:
                    ennemyType = 0;
                    break;
                case var _ when (ennemyType >= 70 && ennemyType < 100):
                    ennemyType = 1;
                    break;
                default:
                    ennemyType = 0;
                    break;
            }

            int rndIdx = Random.Range(0, avalaiblePoints.Count);
            Transform selectedPoint = avalaiblePoints[rndIdx];

            Instantiate(prefMob[ennemyType], selectedPoint.position, Quaternion.identity);

            avalaiblePoints.RemoveAt(rndIdx);
        }
    }
}