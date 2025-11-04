using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public class PropertyList
    {
        private readonly Dictionary<Type, PropertySnapshot> m_properties;

        public PropertyList() => m_properties = new Dictionary<Type, PropertySnapshot>();
        private PropertyList(Dictionary<Type, PropertySnapshot> properties) => m_properties = properties;

        public void Add(PropertySnapshot property) => m_properties[property.GetType()] = property;

        public T Get<T>() where T : PropertySnapshot => (T)m_properties[typeof(T)];

        public PropertySnapshot this[Type type]
        {
            get => m_properties[type];
            set => m_properties[type] = value;
        }

        public PropertyList DeepCopy()
        {
            Dictionary<Type, PropertySnapshot> copy = m_properties.ToDictionary(kvp => kvp.Key,kvp => kvp.Value);
            return new PropertyList(copy);
        }
    }
}
