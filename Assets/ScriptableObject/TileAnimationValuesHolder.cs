using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileAnimationValuesHolder", fileName = "AnimationValues")]
public class TileAnimationValuesHolder : ScriptableObject
{
    [SerializeField] private float timeBetweenSpawns = .15f;
    [SerializeField] private float timeBetweenSingleSpawns = .15f;
    [SerializeField] private float columnFallSpeed = 12f;
    
    [SerializeField] private Ease ease;

    public float TimeBetweenSpawns => timeBetweenSpawns;
    public float TimeBetweenSingleSpawns => timeBetweenSingleSpawns;
    public float ColumFallSpeed => columnFallSpeed;

    public Ease Ease => ease;
}