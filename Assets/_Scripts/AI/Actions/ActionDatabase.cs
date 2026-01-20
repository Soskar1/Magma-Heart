using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MagmaHeart.AI.Actions
{
    public class ActionDatabase
    {
        private readonly TypeMap<UnitAction> m_actions = new();
        public IEnumerable<UnitAction> AllActions => m_actions;

        public T Get<T>() where T : UnitAction => m_actions.Get<T>();
        public UnitAction Get(Type type) => m_actions.Get(type);

        public ActionDatabase(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            RegisterActionsFromAssembly(assembly);
        }

        private void RegisterActionsFromAssembly(Assembly assembly)
        {
            Type actionType = typeof(UnitAction);

            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && actionType.IsAssignableFrom(t));

            foreach (Type type in types)
                if (type.IsSubclassOf(actionType) && !type.IsAbstract && !type.IsNested)
                    if (Activator.CreateInstance(type) is UnitAction actionInstance)
                        m_actions.Add(type, actionInstance);
        }
    }
}
