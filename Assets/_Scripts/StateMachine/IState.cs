using System.Threading.Tasks;

namespace MagmaHeart.StateMachine
{
    public interface IState
    {
        public Task EnterAsync(StatePayload payload);
        public Task ExitAsync();
    }
}