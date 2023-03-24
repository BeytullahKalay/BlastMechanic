using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ClassicBlockSpriteHolder")]
public class ClassicBlockSpriteHolder : ScriptableObject
{
    public List<ClassicBlockSprites> spritesList = new List<ClassicBlockSprites>();

    public ClassicBlockSprites GetRandomTileSprites()
    {
        return spritesList[Random.Range(0, spritesList.Count)];
    }

    public ClassicBlockSprites GetTileSprite(TileType searchTileType)
    {
        foreach (var spriteTile in spritesList)
        {
            if (spriteTile.TileType == searchTileType)
            {
                return spriteTile;
            }
        }
        return null;
    }
}