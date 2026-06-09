using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Node[,] grid;

    [Header("Configuration")]
    public LayerMask obstacleLayer;
    public float nodeDiameter = 1f;

    private int gridOriginX;
    private int gridOriginY;
    private int gridWidth;
    private int gridHeight;

    public void CreatePathfindingGrid(int minX, int minY, int maxX, int maxY)
    {
        gridOriginX = minX;
        gridOriginY = minY;

        // Calcul la taille totale du tableau
        gridWidth = Mathf.Abs(maxX - minX) + 1;
        gridHeight = Mathf.Abs(maxY - minY) + 1;

        grid = new Node[gridWidth, gridHeight];

        Vector2 checkSize = new Vector2(0.8f, 0.8f);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calcul la position réelle dans le monde
                float worldX = gridOriginX + x + 0.5f;
                float worldY = gridOriginY + y + 0.5f;
                Vector3 worldPosition = new Vector3(worldX, worldY, 0);

                // Scan de l'espace
                bool isWalkable = !Physics2D.OverlapBox(worldPosition, checkSize, 0f, obstacleLayer);

                grid[x, y] = new Node(isWalkable, worldPosition, x, y);
            }
        }

        //Debug.Log($"Grille générée ! Taille : {gridWidth}x{gridHeight}");
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        if (grid == null) return null;

        int x = Mathf.FloorToInt(worldPosition.x) - gridOriginX;
        int y = Mathf.FloorToInt(worldPosition.y) - gridOriginY;

        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);

        return grid[x, y];
    }

    // Récupère les voisins d'une case
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // On évite le noeud lui-même

                int neighbourX = node.gridX + x;
                int neighbourY = node.gridY + y;

                if (neighbourX >= 0 && neighbourX < gridWidth && neighbourY >= 0 && neighbourY < gridHeight)
                {
                    neighbours.Add(grid[neighbourX, neighbourY]);
                }
            }
        }

        return neighbours;
    }

}