using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private float _destroyXPosition = -15f; // Точка за экраном слева, где объект удаляется

    private void Update()
    {
        // Получаем текущую скорость из SpeedManager
        float speed = SpeedManager.Instance != null ? SpeedManager.Instance.CurrentSpeed : 5f;

        // Двигаем объект влево
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Если объект ушел далеко влево, удаляем его
        if (transform.position.x < _destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}