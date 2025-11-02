using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    internal static class StateSnapshotMaker
    {
        public static StateSnapshot CreateStateSnapshot(ICollection<AIUnit> units)
        {
            Dictionary<AIUnit, PropertyList> stateProperties = new Dictionary<AIUnit, PropertyList>();

            foreach (AIUnit unit in units)
            {
                PropertyList unitProperties = unit.GetPropertySnapshots();
                stateProperties[unit] = unitProperties;
            }

            return new StateSnapshot(stateProperties);
        }
    }
}
