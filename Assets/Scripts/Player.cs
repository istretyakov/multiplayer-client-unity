using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    public void SetPosition(float x, float y, float z)
    {
        var position = new Vector3(x, y, z);

        transform.position = position;
    }

    public void Move(float x, float y, float z)
    {
        var movement = new Vector3(x, y, z);

        movement = movement * _speed * Time.deltaTime;

        transform.Translate(movement);
    }
}
