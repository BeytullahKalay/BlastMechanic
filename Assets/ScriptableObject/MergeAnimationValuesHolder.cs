using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MergeAnimationValuesHolder")]
public class MergeAnimationValuesHolder : ScriptableObject
{
    [SerializeField] private float duration = .25f;
    [SerializeField] private Ease ease = Ease.InBack;

    public float Duration => duration;

    public Ease Ease => ease;
}
