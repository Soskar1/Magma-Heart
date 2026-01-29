using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public class ApplyDamageCommandPresenter : IBoardCommandPresenter<ApplyDamageCommand>
    {
        public async Task Present(Room room, ApplyDamageCommand command)
        {
            throw new System.NotImplementedException();
        }

        public Task Present(Room room, IBoardCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
