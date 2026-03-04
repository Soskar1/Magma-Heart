using MagmaHeart.AI;

namespace MagmaHeart.Core.AI
{
    public static class TargetSelectorHelper
    {
        public static AIUnitModel SelectNearestTarget(IBoardGameWorld world, int executorId)
        {
            float minDistance = float.MaxValue;
            AIUnitModel target = null;

            foreach (AIUnitModel unit in world.GetUnits())
            {
                if (unit.Id == executorId)
                    continue;

                if (!world.AreEnemiesToEachOther(unit.Id, executorId))
                    continue;

                float distance = world.GetDistance(executorId, unit.Id);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = unit;
                }
            }

            return target;
        }
    }
}
