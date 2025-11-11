namespace MagmaHeart.Core.CombatSystem
{
    public interface IBattleStartedListener
    {
        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args);
    }
}