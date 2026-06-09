using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnnemisState : ObjectState<EnnemisData>
{
    public float life;

    // Pathfinding
    public Pathfinding pathfinding;
    public List<Node> currentPath;
    public bool isAggro = false;
    public float pathUpdateTimer = 0f;

    // Knockback
    public float knockbackTimer;
}
