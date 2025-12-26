using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveActionResolver : IActionResolver
    {
        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            MoveActionData data = (MoveActionData)definition.Data;

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                Position position = state.GetProperty<Position>(potentialTarget);

                resolvedArgs = new MoveActionArgs(executor, position, data);
                return true;
            }

            resolvedArgs = null;
            return false;
        }
    }
}