using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Plans;

namespace MagmaHeart.AI.Reasoning
{
    public record BestPlan(Plan Plan, ActionArgs Args);
}