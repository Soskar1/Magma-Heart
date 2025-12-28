using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public interface IState
    {
        public Task EnterAsync();
        public Task ExitAsync();
    }
}