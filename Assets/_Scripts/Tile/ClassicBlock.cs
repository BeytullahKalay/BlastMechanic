using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ClassicBlock : MonoBehaviour, ISpawnable, ITile
{
    [SerializeField] private ClassicBlockSpriteHolder classicBlockSpriteHolder;
    [SerializeField] private MergeValuesHolder mergeValuesHolder;

    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform TileTransform { get; set; }
    public TileBlock TileBlock { get; set; }

    private TileGridLayout _tileGridLayout;

    private Pooler _pooler;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;
        _pooler = Pooler.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBlock = new ClassicBlockTile(SpriteRenderer, classicBlockSpriteHolder, gameObject);
    }


    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileBlock.SetGridPosition(x, y, playPositioningAnimation);
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        CheckIsDestroyable();
    }

    private async void CheckIsDestroyable()
    {
        if (_gameManager.SpawnerStates == SpawnerStates.Playable)
        {
            var destructArea = GetDestructArea();
            if (destructArea.Count < 2) return;
            await DestroyTiles(destructArea);
            SpawnExtra(destructArea.Count);
            _tileGridLayout.RefillTileGrid();
        }
    }

    public List<ITile> GetDestructArea()
    {
        var destructArea = new List<ITile>();
        var checkQue = new Queue<ITile>();
        checkQue.Enqueue(this);
        destructArea.Add(this);


        while (checkQue.Count > 0)
        {
            var checkTile = checkQue.Dequeue();

            var neighbours = GetNeighbours(checkTile.TileBlock.GridPosition);

            foreach (var neighbour in neighbours)
            {
                if (destructArea.Contains(neighbour)) continue;
                destructArea.Add(neighbour);
                checkQue.Enqueue(neighbour);
            }
        }

        return destructArea;
    }

    private List<ITile> GetNeighbours(Vector2Int tilePos) 
    {
        var allNeighbours = _tileGridLayout.GetNeighbourOf(tilePos);

        var tileNeighbours = new List<ITile>();

        foreach (var neighbour in allNeighbours)
        {
            if (neighbour.TileBlock.TileType == TileBlock.TileType)
            {
                tileNeighbours.Add(neighbour);
            }
        }

        return tileNeighbours;
    }

    private async Task DestroyTiles(List<ITile> destructArea)
    {
        _gameManager.SpawnerStates = SpawnerStates.OnAnimation;

        if (destructArea.Count >= 4)
        {
            var tasks = new Task[destructArea.Count];

            for (var i = 0; i < destructArea.Count; i++)
            {
                tasks[i] = PlayMergeAnimation(destructArea[i]);
            }

            await Task.WhenAll(tasks);
        }
        else
        {
            foreach (var tile in destructArea)
            {
                tile.Destroy();
            }

            await Task.CompletedTask;
        }

        _gameManager.SpawnerStates = SpawnerStates.Playable;
    }

    private async Task PlayMergeAnimation(ITile t)
    {
        var mySequence = DOTween.Sequence();

        mySequence.Append(t.TileTransform.DOMove(transform.position, mergeValuesHolder.Duration)
            .SetEase(mergeValuesHolder.Ease));

        mySequence.Join(t.TileTransform.DOPunchScale(Vector3.one, mergeValuesHolder.Duration, 2));

        await mySequence.OnComplete(t.Destroy).AsyncWaitForCompletion();
    }

    public void Destroy()
    {
        _tileGridLayout.SetGridNull(TileBlock.GridPosition);
        _pooler.TilePool.Release(gameObject);
    }

    private void SpawnExtra(int mergeAmount)
    {
        GameObject extraObj;
        switch (mergeAmount)
        {
            case >= 9:
                extraObj = Instantiate(mergeValuesHolder.DiscoBall, transform.position, Quaternion.identity);
                break;
            case >= 7:
                extraObj = Instantiate(mergeValuesHolder.Bomb, transform.position, Quaternion.identity);
                break;
            case >= 4:
                extraObj = Instantiate(mergeValuesHolder.Rocket, transform.position, Quaternion.identity);
                break;
            case < 4: print("less than 4");
                return;
        }


        var gridPos = TileBlock.GridPosition;
        _tileGridLayout.Grid[gridPos.x, gridPos.y] = extraObj.GetComponent<ITile>();
    }

    public void OnSpawn(Vector2Int spawnPosition, Transform parent)
    {
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        transform.SetParent(parent);
        SetGridPosition(spawnPosition.x, spawnPosition.y);
        _tileGridLayout.Grid[spawnPosition.x, spawnPosition.y] = this;
    }

    public void OnSpawn(Vector2 spawnPosition, Vector2Int movePosition, Transform parent)
    {
        transform.position = spawnPosition;
        transform.SetParent(parent);
        SetGridPosition(movePosition.x, movePosition.y, true);
        _tileGridLayout.Grid[movePosition.x, movePosition.y] = this;
    }
}