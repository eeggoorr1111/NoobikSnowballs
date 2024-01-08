using Narratore.Data;
using UnityEngine;
using Narratore.Solutions.Battle;

public class MovableBot : MonoBehaviour
{
    public Transform Root => _root;
    public float MaxSpeedMove => _maxMoveSpeed;
    public float MinSpeedMove => _minMoveSpeed;
    public float MaxRotateSpeed => _maxRotateSpeed;
    public float MinRotateSpeed => _minRotateSpeed;
    public Vector2 AxselerationRange => _axselerationRange;
    public ClampFloatStat Speed => _speed;
    public bool IsStun { get; set; }


    [SerializeField] private Transform _root;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _minMoveSpeed;
    [SerializeField] private float _maxRotateSpeed;
    [SerializeField] private float _minRotateSpeed;
    [SerializeField] private Vector2 _axselerationRange;
    [SerializeField] private ClampFloatStat _speed;
    [SerializeField] private float _defaultStun;
    
}
