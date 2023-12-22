using Narratore.Data;
using UnityEngine;

public class BotRoster : MonoBehaviour
{
    public Transform Root => _root;
    public float MaxSpeedMove => _maxMoveSpeed;
    public float MinSpeedMove => _minMoveSpeed;
    public float MaxRotateSpeed => _maxRotateSpeed;
    public float MinRotateSpeed => _minRotateSpeed;
    public Vector2 AxselerationRange => _axselerationRange;
    public Value<float> Speed => _speed;


    [SerializeField] private Transform _root;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _minMoveSpeed;
    [SerializeField] private float _maxRotateSpeed;
    [SerializeField] private float _minRotateSpeed;
    [SerializeField] private Vector2 _axselerationRange;
    [SerializeField] private Value<float> _speed;
}
