using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] _cloudPrefabs; // Сюда перетащите префабы ваших двух облаков

    [Header("Spawn Intervals")]
    [SerializeField] private float _minSpawnDelay = 3f;  // Минимальная пауза перед новым облаком
    [SerializeField] private float _maxSpawnDelay = 8f;  // Максимальная пауза

    [Header("Spawn Position Settings")]
    [SerializeField] private float _minHeightY = 1f;     // Минимальная высота полета
    [SerializeField] private float _maxHeightY = 4f;     // Максимальная высота полета

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

        // Создаем несколько облаков сразу при старте, чтобы небо не было пустым
        SpawnInitialClouds();
        ScheduleNextSpawn();
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnCloud();
            ScheduleNextSpawn();
        }
    }

    private void SpawnCloud()
    {
        if (_cloudPrefabs == null || _cloudPrefabs.Length == 0 || _mainCamera == null) return;

        // Случайно выбираем одно из облаков из массива
        int randomIndex = Random.Range(0, _cloudPrefabs.Length);
        GameObject selectedCloud = _cloudPrefabs[randomIndex];

        // Точка спавна чуть правее экрана
        float spawnX = _mainCamera.transform.position.x + _camHorizontalExtent + 2f;
        float spawnY = Random.Range(_minHeightY, _maxHeightY);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, selectedCloud.transform.position.z);
        
        // Создаем облако на сцене
        Instantiate(selectedCloud, spawnPosition, Quaternion.identity, transform);
    }

    private void ScheduleNextSpawn()
    {
        float delay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        _nextSpawnTime = Time.time + delay;
    }

    private void SpawnInitialClouds()
    {
        if (_cloudPrefabs == null || _cloudPrefabs.Length == 0 || _mainCamera == null) return;

        int initialCloudsCount = Random.Range(2, 5);

        for (int i = 0; i < initialCloudsCount; i++)
        {
            int randomIndex = Random.Range(0, _cloudPrefabs.Length);
            GameObject selectedCloud = _cloudPrefabs[randomIndex];

            float spawnX = _mainCamera.transform.position.x + Random.Range(-_camHorizontalExtent, _camHorizontalExtent);
            float spawnY = Random.Range(_minHeightY, _maxHeightY);

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, selectedCloud.transform.position.z);
            Instantiate(selectedCloud, spawnPosition, Quaternion.identity, transform);
        }
    }
}