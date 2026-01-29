using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    public abstract class TurnContext
    {
        public abstract IEnumerable<IBoardCommand> StartTurn(AIUnitModel model);
    }
}
