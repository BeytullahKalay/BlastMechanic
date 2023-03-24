using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ClassicBlock : MonoBehaviour, ISpawnable, ITile
{
    [SerializeField] private TileSpriteHolder tileSpriteHolder;

    public TileData TileData { get; set; }
    
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
        TileData = new TileData(SpriteRenderer, tileSpriteHolder, gameObject);
    }


    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileData.SetGridPosition(x, y, playPositioningAnimation);
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        EventManager.TileClicked?.Invoke(TileData.GridPosition);
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
            
            var neighbours = GetNeighbours(checkTile.TileData.GridPosition);

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
        _tileGridLayout.SetGridNull(TileData.GridPosition);
        _pooler.TilePool.Release(gameObject);
    }

    private List<ITile> GetNeighbours(Vector2Int tilePos)
    {
        var allNeighbours = _tileGridLayout.GetNeighbourOf(tilePos);
        
        var tileNeighbours = new List<ITile>();

        foreach (var neighbour in allNeighbours)
        {
            if (neighbour.TileData.TileType == TileData.TileType)
            {
                tileNeighbours.Add(neighbour);
            }
        }

        return tileNeighbours;
    }
}