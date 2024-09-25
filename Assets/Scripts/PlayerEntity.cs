using UnityEngine;

public class PlayerEntity
{
    public PlayerEntity(GameObject gameObject, int id, Vector3 position)
    {
        GameObject = gameObject;
        Id = id;
        Position = position;
    }

    public GameObject GameObject { get; }

    public int Id { get; }

    public Vector3 Position { get; set; }

    public Player Player { get { return GameObject.GetComponent<Player>(); } }
}
