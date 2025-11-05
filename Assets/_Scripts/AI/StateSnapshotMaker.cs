using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI
{
    internal static class StateSnapshotMaker
    {
        public static StateSnapshot CreateStateSnapshot(ICollection<AIUnit> units)
        {
            Dictionary<AIUnit, TypeMap<PropertySnapshot>> stateProperties = new Dictionary<AIUnit, TypeMap<PropertySnapshot>>();

            foreach (AIUnit unit in units)
            {
                TypeMap<PropertySnapshot> unitProperties = unit.GetPropertySnapshots();
                stateProperties[unit] = unitProperties;
            }

            return new StateSnapshot(stateProperties);
        }
    }
}
