using MagmaHeart.StateMachine;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine.States
{
    public class GameOverState : IState
    {
        public Task EnterAsync(StatePayload payload)
        {
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
