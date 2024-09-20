using Narratore.DI;
using Narratore.Solutions.Battle;

namespace Narratore
{
    public class NNY_BattleData : BattleData
    {
        public Entity<BotShootingConfig> EntityBotShoot { get; set; }
        public Entity<MovableBot> EntityMovableBot { get; set; }
    }
}

