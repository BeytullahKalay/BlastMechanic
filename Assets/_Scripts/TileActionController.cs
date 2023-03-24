using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TileActionController : MonoBehaviour
{
    [SerializeField] private MergeAnimationValuesHolder mergeAnimationValuesHolder;

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

    private async void TileClicked(Vector2Int clickedTileCoord)
    {
        if (_gameManager.SpawnerStates == SpawnerStates.Playable)
        {
            var destructArea = _tileGridLayout.Grid[clickedTileCoord.x, clickedTileCoord.y].GetDestructArea();

            if (destructArea.Count < 2) return;

            await DestroyTiles(destructArea, clickedTileCoord);
            
            _tileGridLayout.RefillTileGrid();
        }
    }

    private async Task DestroyTiles(List<ITile> destructArea, Vector2Int clickedTileCoord)
    {
        _gameManager.SpawnerStates = SpawnerStates.OnAnimation;

        if (destructArea.Count >= 4)
        {
            var tasks = new Task[destructArea.Count];

            for (var i = 0; i < destructArea.Count; i++)
            {
                tasks[i] = PlayMergeAnimation(clickedTileCoord, destructArea[i]);
            }

            await Task.WhenAll(tasks);
        }
        else
        {
            foreach (var t in destructArea)
            {
                t.Destroy();
            }

            await Task.CompletedTask;
        }

        _gameManager.SpawnerStates = SpawnerStates.Playable;
    }

    private async Task PlayMergeAnimation(Vector2Int clickedTileCoord, ITile t)
    {
        var mySequence = DOTween.Sequence();

        mySequence.Append(t.TileTransform.DOMove(new Vector3(clickedTileCoord.x, clickedTileCoord.y, 0),
                mergeAnimationValuesHolder.Duration).SetEase(mergeAnimationValuesHolder.Ease));
        
        mySequence.Join(t.TileTransform.DOPunchScale(Vector3.one, mergeAnimationValuesHolder.Duration, 2));
        
        await mySequence.OnComplete(t.Destroy).AsyncWaitForCompletion();
    }
}