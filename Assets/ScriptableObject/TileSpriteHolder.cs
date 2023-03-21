using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileSpriteHolder")]
public class TileSpriteHolder : ScriptableObject
{
    public List<TileSprites> spritesList = new List<TileSprites>();

    public TileSprites GetRandomTileSprites()
    {
        return spritesList[Random.Range(0, spritesList.Count)];
    }
}
