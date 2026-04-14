using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private GameObject[] roomPrefab;
    [SerializeField] private Direction direction;
    private GameObject grid;

    private enum Direction{
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.FindWithTag("Grid");
        int roomNumber = Random.Range(0, roomPrefab.Length);
        GameObject newRoom = Instantiate(roomPrefab[roomNumber], transform.position, transform.rotation, grid.transform);
        newRoom.name = "Room " + transform.position.x.ToString() + "," + transform.position.y.ToString();
        //GameManager.instance.addRoomInTab(newRoom);
    }
}