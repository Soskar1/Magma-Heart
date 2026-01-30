using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Dungeon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionRunner
    {
        private readonly CommandRunner m_commandRunner;
        private readonly Dictionary<Type, IBoardCommandPresenter> m_presenters = new();

        public ActionRunner()
        {
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

        public void RegisterPresenter<T>(IBoardCommandPresenter presenter) where T : IBoardCommand => m_presenters[typeof(T)] = presenter;
        private IBoardCommandPresenter GetPresenter(IBoardCommand cmd) => m_presenters.TryGetValue(cmd.GetType(), out var p) ? p : null;
    }
}
