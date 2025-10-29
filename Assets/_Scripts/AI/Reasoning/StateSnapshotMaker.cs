using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    internal static class StateSnapshotMaker
    {
        public static StateSnapshot CreateStateSnapshot(ICollection<AIUnit> units)
        {
            Dictionary<AIUnit, Dictionary<Type, PropertySnapshot>> stateProperties = new Dictionary<AIUnit, Dictionary<Type, PropertySnapshot>>();

            foreach (AIUnit unit in units)
            {
                Dictionary<Type, PropertySnapshot> unitProperties = unit.GetPropertySnapshots();
                stateProperties[unit] = unitProperties;
            }

            return new StateSnapshot(stateProperties);
        }
    }
}
