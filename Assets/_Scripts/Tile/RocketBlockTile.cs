using UnityEngine;

public class RocketBlockTile : TileBase
{
    private Sprite _tileSprite;
    public RocketBlockTile(SpriteRenderer spriteRenderer, Sprite tileSprite, GameObject gameObject,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        _object = gameObject;
        _tileSprite = tileSprite;
        Initialize();
    }
    
    public override void Initialize()
    {
        _spriteRenderer.sprite = _tileSprite;
    }
}
