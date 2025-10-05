namespace MagmaHeart.Core.StateMachines
{
    public interface IState
    {
        public void Enter(params object[] args);
        public void Exit();
    }
}