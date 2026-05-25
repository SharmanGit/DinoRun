using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ObstacleData
    {
        public GameObject prefab;
        public float heightOffset; 
    }

    [SerializeField] private ObstacleData[] obstacles;

    [Header("Настройки Спавна")]
    [SerializeField] private float minSpawnDelay = 1.2f;
    [SerializeField] private float maxSpawnDelay = 2.5f;

    private float nextSpawnTime;

    private void Start()
    {
        ScheduleNextSpawn();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            ScheduleNextSpawn();
        }
    }

    private void SpawnObstacle()
    {
        if (obstacles.Length == 0) return;

        // Выбираем случайное препятствие
        int index = Random.Range(0, obstacles.Length);
        ObstacleData obstacle = obstacles[index];

        // Определяем точку спавна
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += obstacle.heightOffset;

        // Создаем объект
        Instantiate(obstacle.prefab, spawnPosition, Quaternion.identity);
    }

    private void ScheduleNextSpawn()
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);

        // Корректируем задержку в зависимости от скорости, 
        // чтобы при высокой скорости препятствия не спавнились слишком часто/редко
        if (SpeedManager.Instance != null)
        {
            // Примерный коэффициент масштабирования задержки
            float speedFactor = 5f / SpeedManager.Instance.CurrentSpeed;
            delay *= Mathf.Clamp(speedFactor, 0.5f, 1f);
        }

        nextSpawnTime = Time.time + delay;
    }
}