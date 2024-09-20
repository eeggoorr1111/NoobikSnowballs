using Narratore.DI;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


namespace Narratore
{
    public class NNY_BattleConfigurator : BattleConfigurator<NNY_BattleData>
    {
        [SerializeField] private EntitiesConfigurator<NNY_BattleData> _target;


        protected override void RegisterDataImpl(IContainerBuilder builder)
        {
            builder.RegisterInstance(Data.EntityBotShoot = new Entity<BotShootingConfig>()).AsSelf().As<IEntity<BotShootingConfig>>();
            builder.RegisterInstance(Data.EntityMovableBot = new Entity<MovableBot>()).AsSelf().As<IEntity<MovableBot>>();
            builder.RegisterInstance(Data.Shops = new ObservableList<ShopRoster>()).AsSelf().As<IObservableList<ShopRoster>>();
        }

        protected override CombineDamageSource CreateDamageSource() =>
             new CombineDamageSource(DamageImpactHandler, ExplosionImpactHandler, FallImpactHandler);

        protected override void CreateMechanics(IContainerBuilder builder, GameStateEventsHandlersDi handlers)
        {

        }

        protected override void CreateUpAbstractionMechanics(IContainerBuilder builder, GameStateEventsHandlersDi handlers)
        {

            //builder.RegisterInstance(_target.Counter);
        }

        protected override IReadOnlyList<IDeathInfo> GetDeathInfo(GameStateEventsHandlersDi handlers)
        {
            return new List<IDeathInfo>()
            {
                DamageImpactHandler.DeathInfo,
                SpikesImpactHandler.DeathInfo,
                HeadChopImpactHandler.DeathInfo,
                FallImpactHandler.DeathInfo,
                ExplosionImpactHandler.DeathInfo,
                TransfigurationImpactHandler.DeathInfo,
                DeathImpactHandler.DeathInfo
            };
        }

        protected override IReadOnlyList<IToEntityImpactHandler> GetToEntityImpactHanders()
        {
            return new List<IToEntityImpactHandler>()
            {
                ExplosionImpactHandler,
                ShootGunImpactHandler,
                DamageImpactHandler,
                HeadChopImpactHandler,
                ChopImpactHandler,
                TransfigurationImpactHandler,
                DeathImpactHandler,
                SpikesImpactHandler,
                ChangeOwnerImpactHandler,
                TeleportImpactHandler,
                ThrowImpactHandler,
                Create2dAreaImpactHandler
            };
        }

        protected override IReadOnlyList<IRevengeImpactHandler> GetRevengeImpactHandlers()
        {
            return new List<IRevengeImpactHandler>()
            {
                ExplosionImpactHandler,
                ShootGunImpactHandler,
                DeathImpactHandler,
                TeleportImpactHandler,
                ThrowImpactHandler,
                Create2dAreaImpactHandler
            };
        }

        protected override IReadOnlyList<IToSpaceImpactHandler> GetToNothingImpactHandlers()
        {
            return new List<IToSpaceImpactHandler>()
            {
                ExplosionImpactHandler,
                ShootGunImpactHandler,
                TeleportImpactHandler,
                Create2dAreaImpactHandler
            };
        }
    }
}

