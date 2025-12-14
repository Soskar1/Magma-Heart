using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    internal class ActionDatabase
    {
        private readonly TypeMap<UnitAction> m_actions = new();
        public IEnumerable<UnitAction> AllActions => m_actions;

        public UnitAction Get<T>() where T : UnitAction => m_actions.Get<T>();

        public ActionDatabase()
        {
            RegisterAllActions();
        }

        private void RegisterAllActions()
        {
            Type actionType = typeof(UnitAction);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && actionType.IsAssignableFrom(t));

            foreach (Type type in types)
                if (type.IsSubclassOf(actionType) && !type.IsAbstract)
                    if (Activator.CreateInstance(type) is UnitAction actionInstance)
                        m_actions.Add(actionInstance);
        }
    }
}
