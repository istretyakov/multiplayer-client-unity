using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
     private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetPosition(float x, float y, float z)
    {
        var position = new Vector3(x, y, z);

        transform.position = position;
    }

    public void Move(float x, float y, float z)
    {
        var movement = new Vector3(x, y, z);

        movement = movement * _speed;

        movement.y = _rigidbody.velocity.y;

        _rigidbody.velocity = movement;
    }
}
