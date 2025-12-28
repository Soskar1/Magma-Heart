using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines.States
{
    public class GameOverState : IState
    {
        public Task EnterAsync()
        {
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
