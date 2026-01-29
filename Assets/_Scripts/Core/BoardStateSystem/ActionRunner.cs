using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionRunner
    {
        private readonly MoveCommandPresenter m_movePresenter;
        private readonly ApplyDamageCommandPresenter m_applyDamagePresenter;
        private readonly CommandRunner m_commandRunner;

        public ActionRunner(MoveCommandPresenter movePresenter, ApplyDamageCommandPresenter applyDamagePresenter)
        {
            m_movePresenter = movePresenter;
            m_applyDamagePresenter = applyDamagePresenter;
            m_commandRunner = new CommandRunner();
        }

        public async Task ApplyAsync(Room room, IEnumerable<IBoardCommand> commands, CancellationToken cancellationToken)
        {
            foreach (IBoardCommand command in commands)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                IBoardCommandPresenter presenter = GetPresenter(command);
                m_commandRunner.Apply(room, command);

                if (presenter != null)
                    await presenter.Present(room, command);
            }
        }

        private IBoardCommandPresenter GetPresenter(IBoardCommand command)
        {
            if (command is MoveCommand)
                return m_movePresenter;
            else if (command is ApplyDamageCommand)
                return m_applyDamagePresenter;

            return null;
        }
    }
}
