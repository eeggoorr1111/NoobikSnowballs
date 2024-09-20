using Narratore;
using Narratore.Solutions.Battle;

public class BotsShooting : EntitiesAspectsObserver<BotShootingConfig>, IBeginnedTickable
{
    public BotsShooting(IEntity<BotShootingConfig> target) : base(target)
    {
        _configs = target;
    }


    private readonly IEntity<BotShootingConfig> _configs;


    public void Tick()
    {
        foreach (var pair in _configs.All)
        {
            Gun gun = pair.Value.Gun;
            if (gun.IsCanTodoAction)
                gun.ShootMagazine();
        }
    }
}
