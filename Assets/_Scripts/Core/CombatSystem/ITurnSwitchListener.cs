namespace MagmaHeart.Core.CombatSystem
{
    public interface ITurnSwitchListener
    {
        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args);
    }
}
