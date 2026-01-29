using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public interface IBoardCommandPresenter
    {
        public Task Present(Room room, IBoardCommand command);
    }

    public interface IBoardCommandPresenter<ICommand> : IBoardCommandPresenter where ICommand : IBoardCommand
    {
        public Task Present(Room room, ICommand command);
    }
}
