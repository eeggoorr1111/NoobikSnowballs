using Narratore.Solutions.Battle;
using UnityEngine;

[System.Serializable]
public class ShootingPushConfig
{
    public ShootingPushConfig(ReadShell shell, EntityRoster entity, float stunDuration, float pushDistance)
    {
        _shell = shell;
        _entity = entity;
        _pushDuration = stunDuration;
        _pushDistance = pushDistance;
    }

    private ShootingPushConfig() { }


    public Component Shell => _shell;
    public Component Entity => _entity;
    public float PushDuration => _pushDuration;
    public float PushDistance => _pushDistance;


    [SerializeField] [HideInInspector] private string _key;
    [SerializeField] private ReadShell _shell;
    [SerializeField] private EntityRoster _entity;
    [SerializeField] private float _pushDuration;
    [SerializeField] private float _pushDistance;

    public void OnValidate()
    {
        _key = $"{_shell.name}->{_entity.name}";
    }
}
