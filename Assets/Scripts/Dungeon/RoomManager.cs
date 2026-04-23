using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private CinemachineConfiner2D _confiner;
    private List<Collider2D> _activeRooms = new List<Collider2D>();


    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
            _confiner = GetComponent<CinemachineConfiner2D>();
    }

    public void EnterRoom(Collider2D roomCollider)
    {
        if (roomCollider == null) return;
        if (!_activeRooms.Contains(roomCollider))
        {
            _activeRooms.Add(roomCollider);
        }
        UpdateCameraConfiner();
    }

    public void ExitRoom(Collider2D roomCollider)
    {
        if (_activeRooms.Contains(roomCollider))
        {
            _activeRooms.Remove(roomCollider);
            UpdateCameraConfiner();
        }
    }

    private void UpdateCameraConfiner()
    {
        if (_confiner == null) _confiner = GetComponent<CinemachineConfiner2D>();

        if (_activeRooms.Count > 0)
        {
            Collider2D latestRoom = _activeRooms[_activeRooms.Count - 1];

            if (_confiner.BoundingShape2D != latestRoom)
            {
                _confiner.BoundingShape2D = latestRoom;
                _confiner.InvalidateBoundingShapeCache();
            }
        }
    }

    public void SetCameraTarget(Transform playerTransform)
    {
        GetComponent<CinemachineCamera>().Follow = playerTransform;

    }
}