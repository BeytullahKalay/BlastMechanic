using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class TileBlock
{
    public Vector2Int GridPosition;

    protected SpriteRenderer _spriteRenderer;
    public GameObject TileGameObject;

    public TileType TileType { get; protected set; }
    public Tween Tween { get; protected set; }

    public virtual void Initialize() { }

    public void SetGridPosition(int x, int y, bool playAnimation)
    {
        GridPosition = new Vector2Int(x, y);
        _spriteRenderer.sortingOrder = y;
        TileGameObject.name = $"({GridPosition.x},{GridPosition.y})";

        var obj = TileGameObject; // defined for dotween unexpected this object error.

        if (playAnimation)
        {
            Tween?.Kill();
            Tween = obj.transform.DOMoveY(y, 13).SetSpeedBased().SetEase(Ease.InQuad).OnComplete(() =>
            {
                obj.transform.DOMoveY(obj.transform.position.y + .15f, .065f).SetLoops(2, LoopType.Yoyo);
            });
        }
    }
}