using UnityEngine;

public class StoneMover : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float _scrollSpeedMultiplier = 1f; // 1 означает полную скорость земли

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
        float baseSpeed = SpeedManager.Instance != null ? SpeedManager.Instance.CurrentSpeed : 5f;
        float currentSpeed = baseSpeed * _scrollSpeedMultiplier;

        // Двигаем камушек влево
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;

        if (_mainCamera == null || _spriteRenderer == null) return;

        float cameraLeftEdge = _mainCamera.transform.position.x - _camHorizontalExtent;

        // Удаляем камушек, когда он полностью ушел за левый край экрана
        if (_spriteRenderer.bounds.max.x <= cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}