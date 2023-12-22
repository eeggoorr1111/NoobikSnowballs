using Narratore.Enums;
using Narratore.Extensions;
using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using Narratore.AI;
using UnityEngine;


public class EnemiesMover : IUpdatable
{
    public EnemiesMover(IEntitiesAspects<BotRoster> bots, SeekSteering seek, IPlayerUnitRoot playerUnit)
    {
        _bots = bots;
        _rootCharacter = playerUnit.Root;
        _seek = seek;
    }


    private readonly IEntitiesAspects<BotRoster> _bots;
    private readonly Transform _rootCharacter;
    private readonly SeekSteering _seek;
    

    public void Tick()
    {
        Vector2 targetPoint = _rootCharacter.position.To2D(TwoAxis.XZ);
        foreach (var pair in _bots.All)
        {
            BotRoster bot = pair.Value;
            Vector2 position = bot.Root.position.To2D(TwoAxis.XZ);
            Vector2 forward = bot.Root.forward.To2D(TwoAxis.XZ);
            Vector2 seek = _seek.Get(position, targetPoint, forward);

            forward += seek;
            forward = forward.normalized;

            float dot = Vector2.Dot(forward, (targetPoint - position).normalized);
            float axceleration = Mathf.Lerp(bot.AxselerationRange.x, bot.AxselerationRange.y, dot.Normalized(0.95f, 1));

            float newSpeed = Mathf.Clamp(bot.Speed.Get() + axceleration, bot.MinSpeedMove, bot.MaxSpeedMove);
            float rotateSpeed = Mathf.Lerp(bot.MaxRotateSpeed, bot.MinRotateSpeed, newSpeed.Normalized(bot.MinSpeedMove, bot.MaxSpeedMove));
            Vector3 direction3d = Vector3.RotateTowards(bot.Root.forward, forward.To3D(TwoAxis.XZ), rotateSpeed, 1f);
            float distance = (targetPoint - position).magnitude;
            float step = Mathf.Clamp(newSpeed * Time.deltaTime, 0f, distance);

            bot.Speed.Change(newSpeed);
            bot.Root.forward = direction3d;
            bot.Root.position += direction3d * step;
        }
    }
}