using UnityEngine;

public class PlayerEntity
{
    public PlayerEntity(GameObject gameObject, int id, Vector3 position, Vector3 velocity)
    {
        GameObject = gameObject;
        Id = id;
        Position = position;
        Velocity = velocity;
    }

    public GameObject GameObject { get; }

    public int Id { get; }

    public Vector3 Position { get; set; }

    public Vector3 Velocity { get; set; }

    public Player Player { get { return GameObject.GetComponent<Player>(); } }

    public OtherPlayerInput OtherPlayerInput { get { return GameObject.GetComponent<OtherPlayerInput>(); } }
}
