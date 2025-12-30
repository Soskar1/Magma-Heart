using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public interface IState
    {
        public Task EnterAsync(StatePayload payload);
        public Task ExitAsync();
    }
}