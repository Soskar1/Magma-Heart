using MagmaHeart.Core.BoardStateSystem.Actions.Presenters;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionExecutor
    {
        private readonly Dictionary<Type, IBoardCommandPresenter> m_presenters = new();
        private readonly IBoardCommandPresenter m_defaultPresenter;

        public ActionExecutor(IBoardCommandPresenter defaultPresenter)
        {
            m_defaultPresenter = defaultPresenter;
        }

        public async Task ApplyAsync(Room room, IEnumerable<IBoardCommand> commands, CancellationToken cancellationToken)
        {
            foreach (IBoardCommand command in commands)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                IBoardCommandPresenter presenter = GetPresenter(command) ?? m_defaultPresenter;
                await presenter.Present(room, command, cancellationToken);
            }
        }

        public void RegisterPresenter<T>(IBoardCommandPresenter presenter) where T : IBoardCommand => m_presenters[typeof(T)] = presenter;
        private IBoardCommandPresenter GetPresenter(IBoardCommand cmd) => m_presenters.TryGetValue(cmd.GetType(), out var presenter) ? presenter : null;
    }
}
