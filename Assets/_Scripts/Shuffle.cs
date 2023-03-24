using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1.5f;
    private TileGridLayout _tileGridLayout;
    private GameManager _gm;

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        _gm = GameManager.Instance;
    }

    public void ShuffleTiles()
    {
        if(_gm.SpawnerStates == SpawnerStates.OnAnimation) return;
        
        _gm.SpawnerStates = SpawnerStates.OnAnimation;

        var allTiles = new List<ITile>();
        var grid = _tileGridLayout.Grid;

        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                allTiles.Add(grid[x, y]);
                grid[x, y] = null;
            }
        }

        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var tile = allTiles[Random.Range(0, allTiles.Count)];
                allTiles.Remove(tile);

                grid[x, y] = tile;
                tile.TileBase.SetGridPosition(x, y, false);
                tile.TileBase.Tween.Kill();
                tile.TileTransform.DOMove(new Vector3(x, y, 0), animationDuration).SetEase(Ease.InOutBack)
                    .OnComplete(() => { _gm.SpawnerStates = SpawnerStates.Playable; });

                tile.TileTransform.DOShakeRotation(animationDuration * 1.5f,Vector3.forward * 20,20,100);
            }
        }

        EventManager.UpdateAllTileSprites?.Invoke();
    }
}