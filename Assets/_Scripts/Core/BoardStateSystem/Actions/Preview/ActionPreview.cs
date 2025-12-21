using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record ActionPreview(UnitAction Action, ActionArgs Args, int EnergyCost);
}
