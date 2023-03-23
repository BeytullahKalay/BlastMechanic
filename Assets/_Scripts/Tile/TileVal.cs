using DG.Tweening;
using UnityEngine;

public struct TileVal
{
    public Vector2Int GridPosition;

    private SpriteRenderer _spriteRenderer;
    private GameObject _object;
    private TileSpriteHolder _tileSpriteHolder;

    public TileType TileType { get; private set; }
    public Tween Tween { get; set; }

    public TileVal(SpriteRenderer spriteRenderer, TileSpriteHolder tileSpriteHolder, GameObject gameObject,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        _object = gameObject;
        _tileSpriteHolder = tileSpriteHolder;
        TileType = TileType.Blue;
        Tween = null;

        Initialize();
    }

    public void Initialize()
    {
        Tween?.Kill();
        var tileSprite = _tileSpriteHolder.GetRandomTileSprites();
        TileType = tileSprite.TileType;
        _spriteRenderer.sprite = tileSprite.DefaultSprite;
        _spriteRenderer.sortingOrder = GridPosition.y;
        _object.name = $"({GridPosition.x},{GridPosition.y})";
    }

    public void SetGridPosition(int x, int y, bool playAnimation)
    {
        GridPosition = new Vector2Int(x, y);
        _spriteRenderer.sortingOrder = y;
        _object.name = $"({GridPosition.x},{GridPosition.y})";

        var obj = _object; // defined for dotween unexpected this object error.

        if (playAnimation)
        {
            Tween?.Kill();
            Tween = obj.transform.DOMoveY(y, 10).SetSpeedBased().SetEase(Ease.InQuad).OnComplete(() =>
            {
                obj.transform.DOMoveY(obj.transform.position.y + .15f, .065f).SetLoops(2, LoopType.Yoyo);
            });
        }
    }
}