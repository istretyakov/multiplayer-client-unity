using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Update()
    {
        var inputH = Input.GetAxisRaw("Horizontal");
        var inputV = Input.GetAxisRaw("Vertical");

        _player.Move(inputH, 0, inputV);
    }
}
