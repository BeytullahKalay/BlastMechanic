using System.Threading.Tasks;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    [SerializeField] private FuseValuesHolder fuseValuesHolder;

    private TileGridLayout _tileGridLayout;
    private Camera _cam;
    private bool _startFly;

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        _cam = Camera.main;
    }

    private void Update()
    {
        if (!_startFly) return;

        var curTransform = transform;
        
        curTransform.position += curTransform.up * (fuseValuesHolder.MoveSpeed * Time.deltaTime);
        var x = Mathf.RoundToInt(curTransform.position.x);
        var y = Mathf.RoundToInt(transform.position.y);
        var grid = _tileGridLayout.Grid;


        if (x >= 0 && y >= 0 && grid.GetLength(0) > x && grid.GetLength(1) > y && grid[x, y] != null)
        {
            _tileGridLayout.Grid[x, y].Interact();
            EventManager.UpdateAllTileSprites?.Invoke();
        }
    }

    public async Task CheckIsPassedBorders()
    {
        _startFly = true;

        while (true)
        {
            var camViewPos = _cam.WorldToViewportPoint(transform.position);

            if (camViewPos.x is > 1.1f or < -0.1f || camViewPos.y is > 1.1f or < -0.1f)
            {
                await Task.CompletedTask;
                Destroy(gameObject);
                break;
            }

            await Task.Yield();
        }
    }
}