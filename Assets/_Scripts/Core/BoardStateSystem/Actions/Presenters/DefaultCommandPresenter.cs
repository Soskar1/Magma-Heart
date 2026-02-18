using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Presenters
{
    public class DefaultCommandPresenter : IBoardCommandPresenter
    {
        private readonly CommandRunner m_runner;

        public DefaultCommandPresenter(CommandRunner runner) => m_runner = runner;

        public Task Present(Room room, IBoardCommand command, CancellationToken token)
        {
            m_runner.Apply(room, command);
            return Task.CompletedTask;
        }
    }
}
