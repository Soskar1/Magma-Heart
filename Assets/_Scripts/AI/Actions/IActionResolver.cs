using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Actions
{
    public interface IActionResolver
    {
        bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs);
    }
}