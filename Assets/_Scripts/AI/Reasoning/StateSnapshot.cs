using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public record StateSnapshot(Dictionary<AIUnit, Dictionary<Type, PropertySnapshot>> StateProperties)
    {
        public float StaticEvaluation()
        {
            float playerStaticEvaluation = 0;
            float aiStaticEvaluation = 0;

            foreach (var keyValuePair in StateProperties)
            {
                if (keyValuePair.Key.IsPlayer)
                {
                    playerStaticEvaluation += keyValuePair.Value.Sum(x => x.Value.Value * x.Value.Weight);
                }
                else
                {
                    aiStaticEvaluation += keyValuePair.Value.Sum(x => x.Value.Value * x.Value.Weight);
                }
            }

            return aiStaticEvaluation - playerStaticEvaluation;
        }

        public void Add(AIUnit unit, List<PropertySnapshot> properties)
        {
            if (!StateProperties.TryGetValue(unit, out var snapshots))
            {
                snapshots = new Dictionary<Type, PropertySnapshot>();
                StateProperties[unit] = snapshots;
            }

            foreach (PropertySnapshot property in properties)
                Add(unit, property);
        }

        public void Add(AIUnit unit, PropertySnapshot property)
        {
            Type type = property.GetType();

            Dictionary<Type, PropertySnapshot> properties = StateProperties[unit];
            if (properties.TryGetValue(type, out PropertySnapshot existing))
            {
                PropertySnapshot merged = existing.Merge(property);
                properties[type] = merged;
            }
            else
            {
                properties[type] = property;
            }
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
