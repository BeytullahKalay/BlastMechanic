using UnityEngine;

public class CameraPositioning : MonoBehaviour
{
    private void Start()
    {
        var grid = TileGridLayout.Instance.Grid;
        Camera.main.transform.position = new Vector3(grid.GetLength(0) / 2, grid.GetLength(1) / 2, -10);
    }
}