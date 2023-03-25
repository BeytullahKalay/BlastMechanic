using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RocketBlock : MonoBehaviour, ITile
{
    [SerializeField] private ExtrasSpriteHolder extrasSpriteHolder;
    public TileBlock TileBlock { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform TileTransform { get; set; }

    private TileGridLayout _tileGridLayout;
    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBlock = new RocketBlockTile(SpriteRenderer, extrasSpriteHolder.RocketSprite, gameObject,
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
        print("clicked!");
        AttemptToDestroyObject();
    }

    public void AttemptToDestroyObject()
    {
        CheckAction();
    }

    private void CheckAction()
    {
        if (_gameManager.SpawnerStates == SpawnerStates.Playable)
        {
            var destructArea = GetDestructTile();
            foreach (var tile in destructArea)
            {
                tile.Destroy();
            }

            _tileGridLayout.RefillTileGrid();
        }
    }

    private List<ITile> GetDestructTile()
    {
        var xLine = _tileGridLayout.GetXLineOf(TileBlock.GridPosition.y);
        return xLine;
    }

    public void Destroy()
    {
        _tileGridLayout.SetGridNull(TileBlock.GridPosition);
        Destroy(gameObject);
    }
}