
using UnityEngine;

public interface ITile: IDestroyable
{
    public TileBlock TileBlock { get; }
    public SpriteRenderer SpriteRenderer { get; }
    public Transform TileTransform { get; }
    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false);
}
