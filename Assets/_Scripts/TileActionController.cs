using UnityEngine;

public class TileActionController : MonoBehaviour
{
    private GameManager _gameManager;
    private TileGridLayout _tileGridLayout;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;
    }

    private void OnEnable()
    {
        EventManager.TileClicked += TileClicked;
    }

    private void OnDisable()
    {
        EventManager.TileClicked -= TileClicked;
    }

    private void TileClicked(Vector2Int clickedTile)
    {
        if (_gameManager.SpawnerStates == SpawnerStates.Playable)
        {
            var destructArea = _tileGridLayout.Grid[clickedTile.x, clickedTile.y].GetDestructArea();

            if (destructArea.Count < 2) return;

            foreach (var t in destructArea)
            {
                t.Destroy();
            }
            
            _tileGridLayout.RefillTileGrid();

        }
    }




}