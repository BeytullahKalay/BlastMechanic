

using UnityEngine;

public interface ISpawnable
{
    public void OnSpawn(Vector2Int spawnPosition,Transform parent);
    public void OnSpawn(Vector2 spawnPosition, Vector2Int movePosition, Transform parent);
}
