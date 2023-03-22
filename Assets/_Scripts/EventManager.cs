using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static Action<Vector2Int> TileClicked;
    public static Action<List<Tile>> UpdateTileSpritesOf;
    public static Action UpdateAllTileSprites;
}
