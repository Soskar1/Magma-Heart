using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public record StateSnapshot(Dictionary<AIUnit, PropertyList> StateProperties)
    {
        public void Update(AIUnit unit, PropertySnapshot property)
        {
            Type type = property.GetType();
            StateProperties[unit][type] = property;
        }

        public void Update(AIUnit unit, List<PropertySnapshot> properties)
        {
            foreach (PropertySnapshot property in properties)
                Update(unit, property);
        }

        public PropertySnapshot GetProperty(AIUnit unit, Type propertyType) => StateProperties[unit][propertyType];
        public T GetProperty<T>(AIUnit unit) where T : PropertySnapshot => (T)GetProperty(unit, typeof(T));

        public List<AIUnit> GetAllUnits() => StateProperties.Keys.ToList();
    }
}
