using UnityEngine;

public class Goalkeeper : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private float _speed = 30f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirectionButton(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        if (_direction != Vector2.zero)
            _rigidbody.AddForce(_direction * _speed);
    }

}
