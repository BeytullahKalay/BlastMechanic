using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ExtrasSpriteHolder")]
public class ExtrasSpriteHolder : ScriptableObject
{
    [Header("Rocket")] [SerializeField] private Sprite rocketSprite;
    [Header("Bomb")] [SerializeField] private Sprite bombSprite;

    [Header("DiscoBall")] 
    [SerializeField] private Sprite discoBallDefault;

    public Color Blue;
    public Color Green;
    public Color Pink;
    public Color Purple;
    public Color Red;
    public Color Yellow;

    public Sprite RocketSprite => rocketSprite;
    public Sprite BombSprite => bombSprite;
    public Sprite DiscoBallDefault => discoBallDefault;
    
}
