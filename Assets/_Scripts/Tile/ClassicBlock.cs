using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ClassicBlock : MonoBehaviour, ISpawnable, ITile
{
    [SerializeField] private ClassicBlockSpriteHolder classicBlockSpriteHolder;

    public TileBase TileBase { get; set; }
    
    private TileGridLayout _tileGridLayout;

    private Pooler _pooler;
    
    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform TileTransform { get; set; }

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        _pooler = Pooler.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBase = new ClassicBlockTile(SpriteRenderer, classicBlockSpriteHolder, gameObject);
    }


    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileBase.SetGridPosition(x, y, playPositioningAnimation);
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        EventManager.TileClicked?.Invoke(TileBase.GridPosition);
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
            
            var neighbours = GetNeighbours(checkTile.TileBase.GridPosition);

            foreach (var neighbour in neighbours)
            {
                if (destructArea.Contains(neighbour)) continue;
                destructArea.Add(neighbour);
                checkQue.Enqueue(neighbour);
            }
        }
        
        return destructArea;
    }

    public void Destroy()
    {
        _tileGridLayout.SetGridNull(TileBase.GridPosition);
        _pooler.TilePool.Release(gameObject);
    }

    private List<ITile> GetNeighbours(Vector2Int tilePos)
    {
        var allNeighbours = _tileGridLayout.GetNeighbourOf(tilePos);
        
        var tileNeighbours = new List<ITile>();

        foreach (var neighbour in allNeighbours)
        {
            if (neighbour.TileBase.TileType == TileBase.TileType)
            {
                tileNeighbours.Add(neighbour);
            }
        }

        return tileNeighbours;
    }

    public void OnSpawn(Vector2Int spawnPosition, Transform parent)
    {
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        transform.SetParent(parent);
        SetGridPosition(spawnPosition.x, spawnPosition.y);
        _tileGridLayout.Grid[spawnPosition.x,spawnPosition.y] = this;
    }

    public void OnSpawn(Vector2 spawnPosition, Vector2Int movePosition, Transform parent)
    {
        transform.position = spawnPosition;
        transform.SetParent(parent);
        SetGridPosition(movePosition.x,movePosition.y,true);
        _tileGridLayout.Grid[movePosition.x,movePosition.y] = this;
    }
}