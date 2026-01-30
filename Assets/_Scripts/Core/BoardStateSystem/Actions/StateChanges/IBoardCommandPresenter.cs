using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using System;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public interface IBoardCommandPresenter
    {
        public Task Present(Room room, IBoardCommand command);
    }

    public interface IBoardCommandPresenter<TCommand> : IBoardCommandPresenter where TCommand : IBoardCommand
    {
        public Task Present(Room room, TCommand command);

        async Task IBoardCommandPresenter.Present(Room room, IBoardCommand command)
        {
            if (command is not TCommand typed)
                throw new ArgumentException($"Expected {typeof(TCommand).Name} but got {command.GetType().Name}");

            await Present(room, typed);
        }
    }
}
