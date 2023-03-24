
using UnityEngine;

public interface ITile: IDestroyable
{
    public TileData TileData { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform TileTransform { get; set; }

    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false);
}
