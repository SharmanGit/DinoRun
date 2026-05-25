using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float _scrollSpeedMultiplier = 0.15f;

    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private float _camHorizontalExtent;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;

        if (_mainCamera != null)
        {
            _camHorizontalExtent = _mainCamera.orthographicSize * _mainCamera.aspect;
        }
    }

    private void Update()
    {
        // Получаем скорость игры из SpeedManager
        float baseSpeed = SpeedManager.Instance != null ? SpeedManager.Instance.CurrentSpeed : 5f;
        float currentSpeed = baseSpeed * _scrollSpeedMultiplier;

        // Двигаем облако влево
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;

        if (_mainCamera == null || _spriteRenderer == null) return;

        float cameraLeftEdge = _mainCamera.transform.position.x - _camHorizontalExtent;

        // Если облако полностью скрылось слева за экраном, удаляем его
        if (_spriteRenderer.bounds.max.x <= cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}