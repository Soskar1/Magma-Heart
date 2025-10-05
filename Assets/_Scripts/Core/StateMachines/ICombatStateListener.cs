namespace MagmaHeart.Core.StateMachines
{
    public interface ICombatStateListener
    {
        public void EnterCombatState();
        public void ExitCombatState();
    }
}