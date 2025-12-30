using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public interface IState
    {
        public Task EnterAsync();
        public Task PayloadEnterAsync(StatePayload payload);
        public Task ExitAsync();
    }

    public interface IState<TStatePayload> : IState
        where TStatePayload : StatePayload
    {
        public Task PayloadEnterAsync(TStatePayload payload);
    }
}