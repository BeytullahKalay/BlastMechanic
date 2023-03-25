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
            block.GetComponent<ClassicBlock>().TileBlock.Initialize();
            block.gameObject.SetActive(true);
        }, block =>
        {
            block.gameObject.SetActive(false);
        }, block =>
        {
            Destroy(block.gameObject);
        }, false, 100,700);
    }
}
