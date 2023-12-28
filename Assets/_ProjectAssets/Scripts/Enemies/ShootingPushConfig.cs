using Narratore.Solutions.Battle;
using UnityEngine;

[System.Serializable]
public class ShootingPushConfig
{
    public ShootingPushConfig(ReadShell shell, EntityRoster entity, float stunDuration, float pushDistance)
    {
        _shell = shell;
        _entity = entity;
        _stunDuration = stunDuration;
        _pushDistance = pushDistance;
    }

    private ShootingPushConfig() { }


    public Component Shell => _shell;
    public Component Entity => _entity;
    public float StunDuration => _stunDuration;
    public float PushDistance => _pushDistance;


    [SerializeField] private ReadShell _shell;
    [SerializeField] private EntityRoster _entity;
    [SerializeField] private float _stunDuration;
    [SerializeField] private float _pushDistance;
}
