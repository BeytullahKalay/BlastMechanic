using UnityEngine;

[System.Serializable]
public class ClassicBlockSprites
{
    public string name;
    public TileType TileType;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite spriteA;
    [SerializeField] private Sprite spriteB;
    [SerializeField] private Sprite spriteC;

    public Sprite DefaultSprite => defaultSprite;
    public Sprite SpriteA => spriteA;
    public Sprite SpriteB => spriteB;
    public Sprite SpriteC => spriteC;
}
