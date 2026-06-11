using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static PlayerBehaviour player;
    public static Camera mainCamera;

    public static int EnemyCount { get; private set; }
    public static event Action<int> OnEnemyCountChanged; // Appelé à chaque changement du nombre de mobs
    public static event Action OnFloorCleared; // Appelé quand il n'y a plus d'ennemis sur l'étage

    public static void ResetEnemyCount()
    {
        EnemyCount = 0;
        OnEnemyCountChanged?.Invoke(EnemyCount);
    }

    public static void RegisterEnemy()
    {
        EnemyCount++;
        OnEnemyCountChanged?.Invoke(EnemyCount);
    }

    public static void UnregisterEnemy()
    {
        EnemyCount--;
        OnEnemyCountChanged?.Invoke(EnemyCount);

        if(EnemyCount <= 0)
            OnFloorCleared?.Invoke();
    }
}
