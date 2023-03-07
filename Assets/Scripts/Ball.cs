using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    private float _moveSpeed = 20.0f;

    private void Start()
    {
        _rigidbody.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.AddForce(Vector2.down * _moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        GameManager.instance.AddScore();
        Destroy(gameObject);
    }
}
