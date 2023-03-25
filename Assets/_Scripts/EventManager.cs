using System;
using System.Collections.Generic;

public static class EventManager
{
    public static Action<List<ITile>> UpdateTileSpritesOf;
    public static Action UpdateAllTileSprites;
}
