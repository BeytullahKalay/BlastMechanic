using System.Collections.Generic;
using UnityEngine;

public class RocketBlock : MonoBehaviour,ITile
{
    [SerializeField] private ExtrasSpriteHolder extrasSpriteHolder;
    
    private TileGridLayout _tileGridLayout;

    
    public TileBase TileBase { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform TileTransform { get; set; }

    private void Awake()
    {
        _tileGridLayout = TileGridLayout.Instance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TileTransform = transform;
        TileBase = new RocketBlockTile(SpriteRenderer,extrasSpriteHolder.RocketSprite,gameObject);
    }

    public void SetGridPosition(int x, int y, bool playPositioningAnimation = false)
    {
        TileBase.SetGridPosition(x, y, playPositioningAnimation);
    }
    
    

    public List<ITile> GetDestructArea()
    {
        var xLine = _tileGridLayout.GetXLineOf(TileBase.GridPosition);
        return xLine;
    }

    public void AttemptToDestroyObject()
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
