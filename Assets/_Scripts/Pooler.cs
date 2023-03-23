using UnityEngine;
using UnityEngine.Pool;

public class Pooler : MonoSingleton<Pooler>
{
    [SerializeField] private GameObject tilePrefab;
    public ObjectPool<GameObject> TilePool;

    private void Awake()
    {
        TilePool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(tilePrefab);
        }, block =>
        {
            block.GetComponent<Tile>().TileVal.Initialize();
            block.gameObject.SetActive(true);
        }, block =>
        {
            block.gameObject.SetActive(false);
        }, block =>
        {
            Destroy(block.gameObject);
        }, false, 100,500);
    }
}
