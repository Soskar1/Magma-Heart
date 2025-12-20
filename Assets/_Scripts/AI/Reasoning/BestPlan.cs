using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning.Plans;

namespace MagmaHeart.AI.Reasoning
{
    public record BestPlan(Plan Plan, AIUnitModel Target);
}