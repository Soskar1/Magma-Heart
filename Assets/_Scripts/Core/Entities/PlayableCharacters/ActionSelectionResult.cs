using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public record ActionSelectionResult(UnitAction Action, ActionArgs Args, int EnergyCost)
    {
        //public void Execute() => Action.Execute(Args);
    }
}
