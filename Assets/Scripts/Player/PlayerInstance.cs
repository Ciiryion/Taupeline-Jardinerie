using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

        RoomManager.instance.SetCameraTarget(transform);
    }
}
