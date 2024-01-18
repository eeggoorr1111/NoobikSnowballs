namespace Narratore.DI
{
    public class MobileShootingAreaActivator : IBegunGameHandler
    {
        public MobileShootingAreaActivator(IPlayerUnitShooting unit)
        {
            _unit = unit;
        }


        private readonly IPlayerUnitShooting _unit;

        
        public void BegunGame(LevelConfig config)
        {
            foreach (var shootArea in _unit.ShootAreas)
                shootArea.Show();
        }
    }
}

