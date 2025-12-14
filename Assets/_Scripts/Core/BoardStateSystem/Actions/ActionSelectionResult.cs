using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record ActionSelectionResult(UnitAction Action, ActionArgs Args, int EnergyCost)
    {
        //public void Execute() => Action.Execute(Args);
    }
}
