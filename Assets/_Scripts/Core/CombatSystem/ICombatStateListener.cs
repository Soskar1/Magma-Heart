namespace MagmaHeart.Core.CombatSystem
{
    public interface ICombatStateListener
    {
        public void EnterCombatState();
        public void ExitCombatState();
    }
}