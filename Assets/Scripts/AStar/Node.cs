using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;
    public bool isWalkable; // true = sol, false = mur
    public Vector3 worldPosition;

    // Les coûts pour l'algorithme A*
    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
