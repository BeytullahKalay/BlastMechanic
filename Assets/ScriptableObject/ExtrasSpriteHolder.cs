using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ExtrasSpriteHolder")]
public class ExtrasSpriteHolder : ScriptableObject
{
    [Header("Rocket")] [SerializeField] private Sprite rocketSprite;
    [Header("Bomb")] [SerializeField] private Sprite bombSprite;

    public Sprite RocketSprite => rocketSprite;
    public Sprite BombSprite => bombSprite;
}
