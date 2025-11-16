using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public record ActionSelectionResult(Action Action, ActionArgs Args, int EnergyCost)
    {
        public void Execute() => Action.Execute(Args);
    }
}
