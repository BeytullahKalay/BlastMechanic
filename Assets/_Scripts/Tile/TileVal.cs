using UnityEngine;

public struct TileVal
{
    public Vector2Int GridPosition;
    
    
    private SpriteRenderer _spriteRenderer;
    private GameObject _object;
    public TileType TileType { get; private set; }

    public TileVal(SpriteRenderer spriteRenderer,TileSprites tileSprites,GameObject gameObject,Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        _object = gameObject;
        TileType = tileSprites.TileType;
        
        spriteRenderer.sprite = tileSprites.DefaultSprite;
        spriteRenderer.sortingOrder = GridPosition.y;
        gameObject.name = $"({GridPosition.x},{GridPosition.y})";
    }

    public void SetGridPosition(int x, int y)
    {
        GridPosition = new Vector2Int(x, y);
        _spriteRenderer.sortingOrder = y;
        _object.name = $"({GridPosition.x},{GridPosition.y})";
    }


}
