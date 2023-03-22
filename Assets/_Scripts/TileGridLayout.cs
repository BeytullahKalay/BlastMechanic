using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileGridLayout : MonoSingleton<TileGridLayout>
{
    [SerializeField] private int gridX, gridY;
    [SerializeField] private GameObject tilePrefab;
    public Tile[,] Grid;

    [Header("Start Animation Values")]
    [SerializeField] private float timeBetweenSpawns = .15f;
    [SerializeField] private float timeBetweenSingleSpawns = .15f;
    [SerializeField] private float startYPosition = 2f;
    [SerializeField] private float columnFallSpeed = 12f;
    [SerializeField] private Ease ease;

    [Header("Gizmo")] [SerializeField] private bool drawGizmos;


    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;


        // initialize grid layout
        Grid = new Tile[gridX, gridY];

        SetupScene();

        // setup camera
        Camera.main.transform.position = new Vector3(gridX / 2, gridY / 2, -10);
    }

    private void SetupScene()
    {
        StartCoroutine(SetupCO(timeBetweenSpawns));
    }

    private IEnumerator SetupCO(float timeBetweenSpawns)
    {
        _gameManager.SpawnerStates = SpawnerStates.OnSpawn;

        for (var y = 0; y < gridY; y++)
        {
            var column = new GameObject($"Column {y}");
            var spawnPosition = gridY + startYPosition;
            Utilities.SetPositionY(column.transform, spawnPosition);
            column.transform.parent = transform;

            for (var x = 0; x < gridX; x++)
            {
                var tileGameObject = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tileGameObject.transform.SetParent(column.transform);
                Utilities.SetLocalPositionY(tileGameObject.transform);
                var tileScript = tileGameObject.GetComponent<Tile>();
                tileScript.SetGridPosition(x, y);
                Grid[x, y] = tileScript;
            }

            column.transform.DOMoveY(y, columnFallSpeed).SetSpeedBased().SetEase(ease).OnComplete(() =>
            {
                column.transform.DOMoveY(column.transform.position.y + .15f, .05f).SetLoops(2, LoopType.Yoyo);
            });

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        _gameManager.SpawnerStates = SpawnerStates.Wait;
    }

    public List<Tile> GetNeighbourOf(Vector2Int pos)
    {
        var neighbours = new List<Tile>();

        // right
        if (pos.x + 1 < gridX && Grid[pos.x + 1, pos.y] != null)
        {
            neighbours.Add(Grid[pos.x + 1, pos.y]);
        }

        // left
        if (pos.x - 1 >= 0 && Grid[pos.x - 1, pos.y] != null)
        {
            neighbours.Add(Grid[pos.x - 1, pos.y]);
        }

        // above
        if (pos.y + 1 < gridY && Grid[pos.x, pos.y + 1] != null)
        {
            neighbours.Add(Grid[pos.x, pos.y + 1]);
        }

        // below
        if (pos.y - 1 >= 0 && Grid[pos.x, pos.y - 1] != null)
        {
            neighbours.Add(Grid[pos.x, pos.y - 1]);
        }

        return neighbours;
    }

    public void RefillTileGrid()
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

        StartCoroutine(Refill(timeBetweenSingleSpawns));
    }

    private IEnumerator Refill(float timeBetweenSpawns)
    {
        for (var y = 0; y < gridY; y++)
        {
            for (var x = 0; x < gridX; x++)
            {
                if (Grid[x, y] != null) continue;

                var tileGameObject = Instantiate(tilePrefab, new Vector2(x, gridY + startYPosition), Quaternion.identity);
                var tileScript = tileGameObject.GetComponent<Tile>();
                tileScript.SetGridPosition(x, y, true);
                Grid[x, y] = tileScript;

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    public void DeleteTileFromGrid(Vector2Int gridCoord)
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