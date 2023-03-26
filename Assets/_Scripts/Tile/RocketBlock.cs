using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketBlock : MonoBehaviour, ITile
{
    [SerializeField] private ExtrasSpriteHolder extrasSpriteHolder;
    [SerializeField] private GameObject fuseObject;
    public TileBlock TileBlock { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Transform TileTransform { get; private set; }

    private TileGridLayout _tileGridLayout;
    private GameManager _gameManager;
    private RocketType _rocketType;
    private bool _triggeredFromAnotherFuse;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileGridLayout = TileGridLayout.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBlock = new ExtraBlockTile(SpriteRenderer, extrasSpriteHolder.RocketSprite, gameObject,
            new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    private void Start()
    {
        _rocketType = Random.value > .5f ? RocketType.Horizontal : RocketType.Vertical;

        if (_rocketType == RocketType.Vertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
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
            Action();
        }
    }

    private async Task Action()
    {
        await DestroyTiles();
        _tileGridLayout.RefillTileGrid();
    }

    private async Task DestroyTiles()
    {
        _gameManager.SpawnerStates = SpawnerStates.OnAnimation;
        
        gameObject.SetActive(false);
        _tileGridLayout.SetGridNull(TileBlock.GridPosition);


        var obj1 = Instantiate(fuseObject, transform.position, transform.rotation);
        var obj2 = Instantiate(fuseObject, transform.position, transform.rotation * Quaternion.Euler(0, 0, 180));

        var tasks = new Task[2];
        tasks[0] = obj1.GetComponent<Fuse>().CheckIsPassedBorders();
        tasks[1] = obj2.GetComponent<Fuse>().CheckIsPassedBorders();

        await Task.WhenAll(tasks);
        _gameManager.SpawnerStates = SpawnerStates.Playable;
    }

    public void Interact()
    {
        Action();
        
        Destroy(gameObject);
    }
}