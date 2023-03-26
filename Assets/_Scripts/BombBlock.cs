using System.Collections.Generic;
using UnityEngine;

public class BombBlock : MonoBehaviour, ITile
{
    [SerializeField] private ExtrasSpriteHolder extrasSpriteHolder;

    public TileBlock TileBlock { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Transform TileTransform { get; private set; }

    private TileGridLayout _tileGridLayout;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBlock = new ExtraBlockTile(SpriteRenderer, extrasSpriteHolder.BombSprite, gameObject,
            new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileBlock.SetGridPosition(x, y, playPositioningAnimation);
    }

    public List<ITile> GetDestructArea()
    {
        return null;
    }

    private void OnMouseDown()
    {
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        if (_gameManager.SpawnerStates == SpawnerStates.Playable)
        {
            Explosion();
        }
    }

    private void Explosion()
    {

        DestroyTiles();
        _tileGridLayout.RefillTileGrid();
    }

    private void DestroyTiles()
    {
        var grid = _tileGridLayout.Grid;
        var actionHappened = false;

        for (var x = TileBlock.GridPosition.x - 1; x <= TileBlock.GridPosition.x + 1; x++)
        {
            for (var y = TileBlock.GridPosition.y - 1; y <= TileBlock.GridPosition.y + 1; y++)
            {
                if(x == TileBlock.GridPosition.x && y == TileBlock.GridPosition.y) continue;
                if (x >= 0 && y >= 0 && grid.GetLength(0) > x && grid.GetLength(1) > y && grid[x, y] != null)
                {
                    _tileGridLayout.Grid[x, y].Interact();
                    actionHappened = true;
                }
            }
        }

        if (actionHappened)
            EventManager.UpdateAllTileSprites?.Invoke();
        
        _tileGridLayout.SetGridNull(TileBlock.GridPosition);
        Destroy(gameObject);

    }


    public void Interact()
    {
        Explosion();
    }
}