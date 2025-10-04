namespace MagmaHeart.Core.CombatSystem
{
    public interface ICombatTurnSwitchListener
    {
        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args);
    }
}