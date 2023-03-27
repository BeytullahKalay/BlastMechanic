using UnityEngine;

public class DiscoBallBlockTile : TileBlock
{
    private TileType _tileType;
    private Sprite _tileSprite;
    private ExtrasSpriteHolder _extraSpriteHolder;

    public DiscoBallBlockTile(SpriteRenderer spriteRenderer, Sprite tileSprite, GameObject gameTileGameObject,
        TileType tileType,
        ExtrasSpriteHolder extraSpriteHolder,
        Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        SpriteRenderer = spriteRenderer;
        TileGameObject = gameTileGameObject;
        _tileSprite = tileSprite;
        _tileType = tileType;
        _extraSpriteHolder = extraSpriteHolder;
    }

    public override void Initialize()
    {
        SpriteRenderer.sprite = _tileSprite;
        SpriteRenderer.sortingOrder = GridPosition.y;
        var c = SetSpriteRendererMaterialColor();
        SpriteRenderer.color = c;
    }

    private Color SetSpriteRendererMaterialColor()
    {
        switch (_tileType)
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
                return SpriteRenderer.material.color = Color.white;
        }
    }
}