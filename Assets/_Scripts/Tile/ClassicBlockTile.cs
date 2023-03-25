using DG.Tweening;
using UnityEngine;

public class ClassicBlockTile : TileBlock
{
    private ClassicBlockSpriteHolder _classicBlockSpriteHolder;

    public ClassicBlockTile(SpriteRenderer spriteRenderer, ClassicBlockSpriteHolder classicBlockSpriteHolder, GameObject gameTileGameObject,
         Vector2Int gridPosition = default)
    {
        GridPosition = gridPosition;
        _spriteRenderer = spriteRenderer;
        TileGameObject = gameTileGameObject;
        _classicBlockSpriteHolder = classicBlockSpriteHolder;
        TileType = TileType.Blue;
        Tween = null;
        Initialize();
    }

    public override void Initialize()
    {
        Tween?.Kill();
        var tileSprite = _classicBlockSpriteHolder.GetRandomTileSprites();
        TileType = tileSprite.TileType;
        _spriteRenderer.sprite = tileSprite.DefaultSprite;
        _spriteRenderer.sortingOrder = GridPosition.y;
        TileGameObject.name = $"({GridPosition.x},{GridPosition.y})";
    }
}
