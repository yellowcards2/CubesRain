using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnTime = 1f;
    [SerializeField] private int _poolSize = 5;

    private Coroutine _spawningCoroutine;
    private ObjectPool<Cube> _pool;

    private float _sphereRadius = 4f;
    private int _activeCubeCount = 0;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: CreateInstantiate,
            actionOnGet: Enable,
            actionOnRelease: Disable,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            maxSize: _poolSize);
    }

    private void Start()
    {
        _spawningCoroutine = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var spawnWait = new WaitForSeconds(_spawnTime);

        while(enabled)
        {
            yield return spawnWait;

            if (_activeCubeCount < _poolSize)
            {
                _pool.Get();
            }
        }
    }

    private Cube CreateInstantiate()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.Enabled += Release;
        return cube;
    }

    private void Enable(Cube cube)
    {
        cube.transform.position = GetRandomSpawn();
        cube.SetDefault();
        cube.gameObject.SetActive(true);
        _activeCubeCount++;
    }

    private void Disable(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _activeCubeCount--;
    }

    private void Release(Cube cube)
    {
        _pool.Release(cube);
    }

    private void Destroy(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private Vector3 GetRandomSpawn()
    {
        Vector3 randomSphere = Random.insideUnitSphere * _sphereRadius;
        return transform.position + new Vector3(randomSphere.x, 0, randomSphere.z);
    }
}
