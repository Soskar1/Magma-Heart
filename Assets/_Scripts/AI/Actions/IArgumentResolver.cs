using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal interface IArgumentResolver
    {
        public IEnumerable<ActionArgs> Resolve(UnitAction action, AIUnitModel executor, ActionPayload payload, SimulatedBoardState state);
    }
}
