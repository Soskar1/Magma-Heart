using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public record ExecutedTask(UnitAction Action, ActionArgs Args);
}