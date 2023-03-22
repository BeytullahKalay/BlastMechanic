using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeManager : MonoBehaviour
{
    [SerializeField] private TileSpriteHolder tileSpriteHolder;


    private TileGridLayout _tileGridLayout;


    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
    }

    private void OnEnable()
    {
        EventManager.UpdateTileSpritesOf += UpdateSpritesOf;
        EventManager.UpdateAllTileSprites += UpdateAllTileSprites;
    }

    private void OnDisable()
    {
        EventManager.UpdateTileSpritesOf -= UpdateSpritesOf;
        EventManager.UpdateAllTileSprites -= UpdateAllTileSprites;
    }

    private void UpdateSpritesOf(List<Tile> neighbourList)
    {
        var tileSprite = tileSpriteHolder.GetTileSprite(neighbourList[0].TileVal.TileType);
        var s = FindSprite(tileSprite, neighbourList.Count);

        foreach (var tile in neighbourList)
        {
            tile.SpriteRenderer.sprite = s;
        }
    }


    private void UpdateAllTileSprites()
    {
        for (var x = 0; x < _tileGridLayout.Grid.GetLength(0); x++)
        {
            for (var y = 0; y < _tileGridLayout.Grid.GetLength(1); y++)
            {
                if (_tileGridLayout.Grid[x, y] == null) continue;
                var neighbours = _tileGridLayout.Grid[x, y].GetDestructArea();
                var tileSprite = tileSpriteHolder.GetTileSprite(neighbours[0].TileVal.TileType);
                var s = FindSprite(tileSprite, neighbours.Count);

                foreach (var neighbour in neighbours)
                {
                    neighbour.SpriteRenderer.sprite = s;
                }
            }
        }
    }

    private static Sprite FindSprite(TileSprites tileSprite, int count)
    {
        var s = tileSprite.DefaultSprite;

        switch (count)
        {
            case >= 9:
                //sprite C
                s = tileSprite.SpriteC;
                break;
            case >= 7:
                //sprite B
                s = tileSprite.SpriteB;
                break;
            case >= 4:
                //sprite A
                s = tileSprite.SpriteA;
                break;
        }

        return s;
    }
}