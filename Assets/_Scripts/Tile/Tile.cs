using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, ISpawnable, IDestroyable
{
    [SerializeField] private TileSpriteHolder tileSpriteHolder;

    public TileVal TileVal;

    private SpriteRenderer _spriteRenderer;
    private TileGridLayout _tileGridLayout;

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        TileVal = new TileVal(_spriteRenderer, tileSpriteHolder.GetRandomTileSprites(), gameObject);
    }

    public void SetGridPosition(int x, int y,bool playPositioningAnimation = false)
    {
        TileVal.SetGridPosition(x, y,playPositioningAnimation);
    }

    public void PlayTilePositioningAnimation(Vector2Int newPos)
    {
        transform.DOMoveY(newPos.y, 10).SetSpeedBased().SetEase(Ease.InQuad).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y + .15f, .065f).SetLoops(2, LoopType.Yoyo);
        });
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        EventManager.TileClicked?.Invoke(TileVal.GridPosition);
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