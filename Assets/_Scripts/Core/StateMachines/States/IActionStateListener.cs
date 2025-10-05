namespace MagmaHeart.Core.StateMachines
{
    public interface IActionStateListener
    {
        public void EnterActionState();
        public void ExitActionState();
    }
}