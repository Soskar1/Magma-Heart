using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    public record StateSnapshot(Dictionary<AIUnit, TypeMap<PropertySnapshot>> StateProperties, int CurrentSimulationDepth)
    {
        public void Update<T>(AIUnit unit, T property) where T : PropertySnapshot => StateProperties[unit][typeof(T)] = property;
        public T GetProperty<T>(AIUnit unit) where T : PropertySnapshot => (T)StateProperties[unit][typeof(T)];
    }
}
