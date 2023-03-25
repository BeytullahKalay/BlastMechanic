using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MergeValuesHolder")]
public class MergeValuesHolder : ScriptableObject
{
    [SerializeField] private float duration = .25f;
    [SerializeField] private Ease ease = Ease.InBack;

    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject discoBall;

    public float Duration => duration;

    public Ease Ease => ease;

    public GameObject Rocket => rocket;
    public GameObject Bomb => bomb;
    public GameObject DiscoBall=> discoBall;
}
