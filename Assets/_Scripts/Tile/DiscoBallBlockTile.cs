using UnityEngine;

public class DiscoBallBlockTile : TileBlock
{
    public TileType TileType { get; }

    private Sprite _tileSprite;
    private ExtrasSpriteHolder _extraSpriteHolder;

    public DiscoBallBlockTile(SpriteRenderer spriteRenderer, Sprite tileSprite, GameObject gameTileGameObject,
        TileType tileType,
        ExtrasSpriteHolder extraSpriteHolder,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        TileGameObject = gameTileGameObject;
        _tileSprite = tileSprite;
        TileType = tileType;
        _extraSpriteHolder = extraSpriteHolder;
    }

    public override void Initialize()
    {
        _spriteRenderer.sprite = _tileSprite;
        _spriteRenderer.sortingOrder = GridPosition.y;
        var c = SetSpriteRendererMaterialColor();
        _spriteRenderer.color = c;
    }

    private Color SetSpriteRendererMaterialColor()
    {
        switch (TileType)
        {
            case TileType.Red:
                return _extraSpriteHolder.Red;
            case TileType.Green:
                return _extraSpriteHolder.Green;
            case TileType.Yellow:
                return _extraSpriteHolder.Yellow;
            case TileType.Purple:
                return _extraSpriteHolder.Purple;
            case TileType.Pink:
                return _extraSpriteHolder.Pink;
            case TileType.Blue:
                return _extraSpriteHolder.Blue;
            default:
                return _spriteRenderer.material.color = Color.white;
        }
    }
}