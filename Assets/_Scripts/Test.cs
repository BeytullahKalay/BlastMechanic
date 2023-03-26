using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("CALLED!");
            EventManager.UpdateAllTileSprites?.Invoke();
        }
    }
}
