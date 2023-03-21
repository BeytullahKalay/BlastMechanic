using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, ISpawnable, IDestroyable
{
    [SerializeField] private TileSpriteHolder tileSpriteHolder;

    private SpriteRenderer _spriteRenderer;
    private TileVal _tileVal;
    private TileGridLayout _tileGridLayout;

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tileVal = new TileVal(_spriteRenderer, tileSpriteHolder.GetRandomTileSprites(), gameObject);
    }

    public void SetGridPosition(int x, int y)
    {
        _tileVal.SetGridPosition(x, y);
    }

    public void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        EventManager.TileClicked?.Invoke(_tileVal.GridPosition);
    }

    public List<IDestroyable> GetDestructArea()
    {
        var destructArea = new List<IDestroyable>();
        var checkQue = new Queue<Tile>();
        checkQue.Enqueue(this);

        while (checkQue.Count > 0)
        {
            var checkTile = checkQue.Dequeue();
            destructArea.Add(checkTile);
            var neighbours = GetNeighbours(checkTile._tileVal.GridPosition);
            
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
        Destroy(gameObject);
    }

    private List<Tile> GetNeighbours(Vector2Int tilePos)
    {
        var allNeighbours = _tileGridLayout.GetNeighbourOf(tilePos);
        var tileNeighbours = new List<Tile>();

        foreach (var neighbour in allNeighbours)
        {
            if (neighbour._tileVal.TileType == _tileVal.TileType)
            {
                tileNeighbours.Add(neighbour);
            }
        }

        return tileNeighbours;
    }
}