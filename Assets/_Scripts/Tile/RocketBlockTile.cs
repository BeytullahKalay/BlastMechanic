using UnityEngine;

public class RocketBlockTile : TileBlock
{
    private Sprite _tileSprite;
    public RocketBlockTile(SpriteRenderer spriteRenderer, Sprite tileSprite, GameObject gameTileObject,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        _tileObject = gameTileObject;
        _tileSprite = tileSprite;
        Initialize();
    }
    
    public override void Initialize()
    {
        _spriteRenderer.sprite = _tileSprite;
    }
}
