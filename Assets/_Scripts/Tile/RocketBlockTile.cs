using UnityEngine;

public class RocketBlockTile : TileBlock
{
    private Sprite _tileSprite;
    public RocketBlockTile(SpriteRenderer spriteRenderer, Sprite tileSprite, GameObject gameTileGameObject,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        TileGameObject = gameTileGameObject;
        _tileSprite = tileSprite;
        Initialize();
    }
    
    public override void Initialize()
    {
        _spriteRenderer.sprite = _tileSprite;
        _spriteRenderer.sortingOrder = GridPosition.y;
    }
}
