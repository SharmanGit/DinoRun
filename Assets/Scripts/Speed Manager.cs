using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance { get; private set; }

    [Header("Настройки Скорости")]
    [SerializeField] private float _initialSpeed = 5f;
    [SerializeField] private float _speedIncreaseRate = 0.1f; 
    [SerializeField] private float _maxSpeed = 20f;

    public float CurrentSpeed { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentSpeed = _initialSpeed;
    }

    private void Update()
    {
        if (CurrentSpeed < _maxSpeed)
        {
            CurrentSpeed += _speedIncreaseRate * Time.deltaTime;
        }
    }
}