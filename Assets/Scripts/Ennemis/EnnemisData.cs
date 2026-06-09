using UnityEngine;

[CreateAssetMenu(menuName = "Entity/EnnemisData")]
public class EnnemisData : ObjectData
{
    public int walkSpeed = 1;
    public int maxLife = 5;
    public float kbForce = 50f;

    [Header("Vision")]
    public float detectionRadius = 5f;
    public float aggroRadius = 12f; // Champ de vision une fois vu
    public LayerMask obstacleLayer;
    public float pathUpdateInterval = 0.2f;

    [Space]
    public Weapon Weapon;
}
