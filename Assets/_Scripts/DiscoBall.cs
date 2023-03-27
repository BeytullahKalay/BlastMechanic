using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, ITile
{
    [SerializeField] private ExtrasSpriteHolder extraSpriteHolder;

    public TileBlock TileBlock { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Transform TileTransform { get; private set; }
    public TileType SearchTileType { get;  set; }

    private TileGridLayout _tileGridLayout;
    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
    }

    public void CreateNewDiscoTileBlock()
    {
        TileBlock = new DiscoBallBlockTile(SpriteRenderer, extraSpriteHolder.DiscoBallDefault, gameObject, SearchTileType,
            extraSpriteHolder, new Vector2Int((int)transform.position.x,(int)transform.position.y));
        
        TileBlock.Initialize();
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
            FindAndDestroy();
            _tileGridLayout.RefillTileGrid();
        }
    }

    private void FindAndDestroy()
    {
        var grid = _tileGridLayout.Grid;
        
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                if(TileBlock.GridPosition == new Vector2Int(x,y)) continue;
                
                if (grid[x,y].TileBlock.TileType == SearchTileType)
                {
                    grid[x, y].Interact();
                }
            }
        }

        _tileGridLayout.SetGridNull(TileBlock.GridPosition);
        _tileGridLayout.RefillTileGrid();
        Destroy(gameObject);

    }

    public void Interact()
    {
        FindAndDestroy();
    }
}