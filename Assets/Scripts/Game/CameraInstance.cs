using UnityEngine;

public class CameraInstance : MonoBehaviour
{
    private void Awake()
    {
        GameManager.mainCamera = GetComponent<Camera>();
    }
}
