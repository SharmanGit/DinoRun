using UnityEngine;

public class DinoController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _jumpForce = 12f;
    
    [Header("Crouch Settings")]
    [SerializeField] private Vector2 _crouchColliderSize = new Vector2(1f, 0.5f);
    [SerializeField] private Vector2 _crouchColliderOffset = new Vector2(0f, -0.25f);

    private Rigidbody2D _rb;
    private BoxCollider2D _normalCollider;
    private bool _isGrounded;
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;

    private void Start()
    {
        // Автоматически находим компоненты на объекте динозавра
        _rb = GetComponent<Rigidbody2D>();
        _normalCollider = GetComponent<BoxCollider2D>();
        
        if (_normalCollider != null)
        {
            _originalColliderSize = _normalCollider.size;
            _originalColliderOffset = _normalCollider.offset;
        }
    }

    private void Update()
    {
        // Прыжок (клавиша Пробел или Стрелка Вверх)
        if (_isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }

        // Приседание (клавиша Стрелка Вниз)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Crouch(true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Crouch(false);
        }
    }

    private void Jump()
    {
        // Сбрасываем вертикальную скорость перед прыжком для стабильности высоты прыжка
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f); 
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Crouch(bool crouch)
    {
        if (_normalCollider == null) return;

        if (crouch)
        {
            // Уменьшаем коллайдер под размеры приседания
            _normalCollider.size = _crouchColliderSize;
            _normalCollider.offset = _crouchColliderOffset;
        }
        else
        {
            // Возвращаем исходный размер коллайдера
            _normalCollider.size = _originalColliderSize;
            _normalCollider.offset = _originalColliderOffset;
        }
    }

    // Проверка приземления на объект с тегом "Ground"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    // Проверка ухода с объекта с тегом "Ground"
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}