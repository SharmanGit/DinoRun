using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [Header("Stone Prefabs")]
    [SerializeField] private GameObject[] _stonePrefabs; // Сюда перетащите 2 префаба ваших камушков

    [Header("Distance Settings (Intervals)")]
    [Tooltip("Минимальная пауза (маленькое расстояние между камнями)")]
    [SerializeField] private float _minSpawnDelay = 2f;  
    [Tooltip("Максимальная пауза (большое расстояние между камнями)")]
    [SerializeField] private float _maxSpawnDelay = 6f;  

    [Header("Position Settings")]
    [Tooltip("Высота земли. Настройте так, чтобы камушки лежали точно на поверхности")]
    [SerializeField] private float _groundHeightY = -3f; 

    private Camera _mainCamera;
    private float _camHorizontalExtent;
    private float _nextSpawnTime;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera != null)
        {
            _camHorizontalExtent = _mainCamera.orthographicSize * _mainCamera.aspect;
        }

        // Спавним стартовые камушки на сцене при запуске
        SpawnInitialStones();
        ScheduleNextSpawn();
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnStone();
            ScheduleNextSpawn();
        }
    }

    private void SpawnStone()
    {
        if (_stonePrefabs == null || _stonePrefabs.Length == 0 || _mainCamera == null) return;

        // Случайно выбираем один из префабов камушков (меняет их порядок)
        int randomIndex = Random.Range(0, _stonePrefabs.Length);
        GameObject selectedStone = _stonePrefabs[randomIndex];

        // Точка спавна справа за экраном
        float spawnX = _mainCamera.transform.position.x + _camHorizontalExtent + 2f;

        // Координаты спавна с фиксированной высотой земли
        Vector3 spawnPosition = new Vector3(spawnX, _groundHeightY, selectedStone.transform.position.z);
        
        Instantiate(selectedStone, spawnPosition, Quaternion.identity, transform);
    }

    private void ScheduleNextSpawn()
    {
        // Случайная задержка определяет случайное расстояние между камушками
        float delay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        _nextSpawnTime = Time.time + delay;
    }

    private void SpawnInitialStones()
    {
        if (_stonePrefabs == null || _stonePrefabs.Length == 0 || _mainCamera == null) return;

        int initialStonesCount = Random.Range(1, 3);

        for (int i = 0; i < initialStonesCount; i++)
        {
            int randomIndex = Random.Range(0, _stonePrefabs.Length);
            GameObject selectedStone = _stonePrefabs[randomIndex];

            float spawnX = _mainCamera.transform.position.x + Random.Range(-_camHorizontalExtent, _camHorizontalExtent);
            Vector3 spawnPosition = new Vector3(spawnX, _groundHeightY, selectedStone.transform.position.z);
            
            Instantiate(selectedStone, spawnPosition, Quaternion.identity, transform);
        }
    }
}