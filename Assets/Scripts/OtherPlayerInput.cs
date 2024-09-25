using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OtherPlayerInput : MonoBehaviour
{
    private Vector3 _lastReceivedPosition;
    private Vector3 _lastReceivedVelocity;

    private List<float> _ticks = new List<float> { 0, 0, 0, 0, 0 };
    private float _tickRate;

    void Update()
    {
        Vector3 extrapolatedPosition = _lastReceivedPosition + _lastReceivedVelocity * _tickRate;
        transform.position = Vector3.MoveTowards(transform.position, extrapolatedPosition, _lastReceivedVelocity.magnitude * Time.deltaTime);
    }

    public void OnReceivePosition(Vector3 position, Vector3 velocity)
    {
        _ticks.Add(Time.deltaTime);
        _ticks.RemoveAt(0);

        _lastReceivedPosition = position;
        _lastReceivedVelocity = velocity;

        _tickRate = _ticks.Average();
    }
}
