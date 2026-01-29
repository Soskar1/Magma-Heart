using MagmaHeart.AI.Boards;
using System.Collections.Generic;

namespace MagmaHeart.AI.Execution
{
    public interface IStartOfTurnCommandFactory
    {
        IEnumerable<IBoardCommand> BuildStartOfTurnCommands(Board board, AIUnitModel unit);
    }
}
