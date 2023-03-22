using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, ISpawnable, IDestroyable
{
    [SerializeField] private TileSpriteHolder tileSpriteHolder;

    public TileVal TileVal;

    public SpriteRenderer SpriteRenderer { get; private set; }
    private TileGridLayout _tileGridLayout;

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileVal = new TileVal(SpriteRenderer, tileSpriteHolder.GetRandomTileSprites(), gameObject);
    }

    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileVal.SetGridPosition(x, y, playPositioningAnimation);
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        EventManager.TileClicked?.Invoke(TileVal.GridPosition);
    }

    public List<Tile> GetDestructArea()
    {
        var destructArea = new List<Tile>();
        var checkQue = new Queue<Tile>();
        checkQue.Enqueue(this);
        destructArea.Add(this);

        while (checkQue.Count > 0)
        {
            var checkTile = checkQue.Dequeue();
            var neighbours = GetNeighbours(checkTile.TileVal.GridPosition);

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
        _tileGridLayout.DeleteTileFromGrid(TileVal.GridPosition);
        Destroy(gameObject);
    }

    private List<Tile> GetNeighbours(Vector2Int tilePos)
    {
        var allNeighbours = _tileGridLayout.GetNeighbourOf(tilePos);
        var tileNeighbours = new List<Tile>();

        foreach (var neighbour in allNeighbours)
        {
            if (neighbour.TileVal.TileType == TileVal.TileType)
            {
                tileNeighbours.Add(neighbour);
            }
        }

        return tileNeighbours;
    }
}