using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileGridLayout : MonoSingleton<TileGridLayout>
{
    public ITile[,] Grid;

    [SerializeField] private int gridX, gridY;
    [SerializeField] private TileAnimationValuesHolder tileAnimationValuesHolder;
    [SerializeField] private float increaseSpawnYPosition = 1f;

    [Header("Gizmo")] [SerializeField] private bool drawGizmos;

    private GameManager _gameManager;
    private Camera _camera;
    private Pooler _pooler;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _camera = Camera.main;
        _pooler = Pooler.Instance;

        // initialize grid layout
        Grid = new ITile[gridX, gridY];

        SetupScene();
    }

    private void SetupScene()
    {
        StartCoroutine(SetupCO(tileAnimationValuesHolder.TimeBetweenSpawns));
    }

    private IEnumerator SetupCO(float timeBetweenSpawns)
    {
        _gameManager.SpawnerStates = SpawnerStates.OnAnimation;

        for (var y = 0; y < gridY; y++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            var column = new GameObject($"Column {y}");
            var spawnPosition = Utilities.GetTopOfScreenY(_camera) + increaseSpawnYPosition;
            Utilities.SetPositionY(column.transform, spawnPosition);
            column.transform.parent = transform;

            for (var x = 0; x < gridX; x++)
            {
                var tileGameObject = _pooler.TilePool.Get();
                var tileScript = tileGameObject.GetComponent<ClassicBlock>();
                tileScript.OnSpawn(new Vector2Int(x, y), column.transform);
                Utilities.SetLocalPositionY(tileGameObject.transform);
                EventManager.UpdateTileSpritesOf?.Invoke(tileScript.GetDestructArea());
            }

            column.transform.DOMoveY(y, tileAnimationValuesHolder.ColumFallSpeed).SetSpeedBased()
                .SetEase(tileAnimationValuesHolder.Ease).OnComplete(() =>
                {
                    column.transform.DOMoveY(column.transform.position.y + .15f, .05f).SetLoops(2, LoopType.Yoyo);
                });

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        _gameManager.SpawnerStates = SpawnerStates.Playable;
    }

    public void RefillTileGrid()
    {
        ShiftTilesDown();
        StartCoroutine(RefillCO(tileAnimationValuesHolder.TimeBetweenSingleSpawns));
    }

    private void ShiftTilesDown()
    {
        for (var x = 0; x < gridX; x++)
        {
            for (var y = 0; y < gridY; y++)
            {
                if (Grid[x, y] == null) continue;

                var emptySpacesUnderTile = 0;

                for (var checkIndex = y; checkIndex >= 0; checkIndex--)
                {
                    if (Grid[x, checkIndex] == null) emptySpacesUnderTile++;
                }

                if (emptySpacesUnderTile > 0)
                {
                    var block = Grid[x, y];

                    block.SetGridPosition(x, y - emptySpacesUnderTile, true);
                    Grid[x, y - emptySpacesUnderTile] = block;
                    Grid[x, y] = null;
                }
            }
        }
    }

    private IEnumerator RefillCO(float timeBetweenSpawns)
    {
        for (var y = 0; y < gridY; y++)
        {
            for (var x = 0; x < gridX; x++)
            {
                if (Grid[x, y] != null) continue;

                var spawnPosition = Utilities.GetTopOfScreenY(_camera) + increaseSpawnYPosition;
                var tileGameObject = _pooler.TilePool.Get();

                tileGameObject.transform.position = new Vector2(x, spawnPosition);
                var tileScript = tileGameObject.GetComponent<ClassicBlock>();
                tileScript.OnSpawn(new Vector2(x, spawnPosition), new Vector2Int(x, y), transform);

                yield return new WaitForSeconds(timeBetweenSpawns);

                EventManager.UpdateAllTileSprites?.Invoke();
            }
        }
    }

    public List<ITile> GetNeighbourOf(Vector2Int pos)
    {
        var neighbours = new List<ITile>();

        if (pos.x + 1 < gridX && Grid[pos.x + 1, pos.y] != null)
        {
            var tile = Grid[pos.x + 1, pos.y];
            var t = tile as ClassicBlock;
            if (t != null) neighbours.Add(tile);
        }

        // left
        if (pos.x - 1 >= 0 && Grid[pos.x - 1, pos.y] != null)
        {
            var tile = Grid[pos.x - 1, pos.y];
            var t = Grid[pos.x - 1, pos.y] as ClassicBlock;
            if (t != null) neighbours.Add(tile);
        }

        // above
        if (pos.y + 1 < gridY && Grid[pos.x, pos.y + 1] != null)
        {
            var tile = Grid[pos.x, pos.y + 1];
            var t = tile as ClassicBlock;
            if (t != null) neighbours.Add(tile);
        }

        // below
        if (pos.y - 1 >= 0 && Grid[pos.x, pos.y - 1] != null)
        {
            var tile = Grid[pos.x, pos.y - 1];
            var t = tile as ClassicBlock;
            if (t != null) neighbours.Add(tile);
        }

        return neighbours;
    }
    
    public void SetGridNull(Vector2Int gridCoord)
    {
        Grid[gridCoord.x, gridCoord.y] = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Gizmos.color = Grid[x, y] == null ? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one * .9f);
            }
        }
    }
}