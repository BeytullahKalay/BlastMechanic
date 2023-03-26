using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FuseValuesHolder", fileName = "FuseValuesHolder" )]
public class FuseValuesHolder : ScriptableObject
{
    [SerializeField] private float moveSpeed;

    public float MoveSpeed => moveSpeed;
}
