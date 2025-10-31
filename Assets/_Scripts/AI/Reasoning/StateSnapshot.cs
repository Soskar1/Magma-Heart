using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public record StateSnapshot(Dictionary<AIUnit, PropertyList> StateProperties)
    {
        public float StaticEvaluation()
        {
            float evaluation = 0;
            foreach (var keyValuePair in StateProperties)
                evaluation += keyValuePair.Key.EvaluateProperties(keyValuePair.Value);

            return evaluation;
        }

        public void Add(AIUnit unit, PropertySnapshot property)
        {
            Type type = property.GetType();
            PropertyList list = StateProperties[unit];

            if (list.TryGet(type, out PropertySnapshot existing))
                property = existing.Merge(property);

            list[type] = property;
        }

        public void Replace(AIUnit unit, PropertySnapshot property)
        {
            Type type = property.GetType();
            StateProperties[unit][type] = property;
        }

        public void Replace(AIUnit unit, List<PropertySnapshot> properties)
        {
            foreach (PropertySnapshot property in properties)
                Replace(unit, property);
        }

        public PropertySnapshot GetProperty(AIUnit unit, Type propertyType) => StateProperties[unit][propertyType];
        public T GetProperty<T>(AIUnit unit) where T : PropertySnapshot => (T)GetProperty(unit, typeof(T));

        public List<AIUnit> GetAllUnits() => StateProperties.Keys.ToList();
    }
}
