using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float _scrollSpeedMultiplier = 1f;

    [Header("Pixel Seams Fix")]
    [Tooltip("Небольшой нахлёст в единицах Unity, чтобы скрыть прозрачные стыки в 1 пиксель")]
    [SerializeField] private float _overlap = 0.05f; 

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

        // Движение строго по мировым координатам
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;

        if (_mainCamera == null || _spriteRenderer == null) return;

        // Левая граница экрана
        float cameraLeftEdge = _mainCamera.transform.position.x - _camHorizontalExtent;

        // Если правый край нашего спрайта ушел за левую границу экрана
        if (_spriteRenderer.bounds.max.x <= cameraLeftEdge)
        {
            RepositionToRightmost();
        }
    }

    private void RepositionToRightmost()
    {
        SpriteRenderer rightmostRenderer = null;
        float rightmostMaxX = _spriteRenderer.bounds.max.x; // Начинаем отсчет с себя

        if (transform.parent != null)
        {
            foreach (Transform sibling in transform.parent)
            {
                // Игнорируем себя и выключенные объекты
                if (sibling != transform && sibling.gameObject.activeSelf)
                {
                    // Проверяем только те объекты, у которых действительно есть картинка!
                    SpriteRenderer siblingRenderer = sibling.GetComponent<SpriteRenderer>();
                    if (siblingRenderer != null)
                    {
                        // Находим тот спрайт, у которого правая граница дальше всего в мире
                        if (siblingRenderer.bounds.max.x > rightmostMaxX)
                        {
                            rightmostMaxX = siblingRenderer.bounds.max.x;
                            rightmostRenderer = siblingRenderer;
                        }
                    }
                }
            }
        }

        if (rightmostRenderer != null)
        {
            // Находим левую границу нашего спрайта
            float currentLeftEdge = _spriteRenderer.bounds.min.x;

            // Вычисляем точный сдвиг, чтобы наше левое ребро совпало с правым ребром соседа
            // Вычитаем _overlap, чтобы спрайты слегка заходили друг на друга и перекрывали прозрачную щель
            float shift = (rightmostMaxX - currentLeftEdge) - _overlap;

            // Перемещаем спрайт
            transform.position = new Vector3(transform.position.x + shift, transform.position.y, transform.position.z);
        }
    }
}