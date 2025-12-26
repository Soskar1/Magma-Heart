using System;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Actions
{
    public class ActionDefinition
    {
        public Type ActionType { get; init; }
        public ActionData Data { get; init; }

        private IActionResolver m_actionResolver;

        public ActionDefinition(Type actionType, ActionData data, IActionResolver actionResolver)
        {
            ActionType = actionType;
            Data = data;
            m_actionResolver = actionResolver;
        }

        public bool TryResolve(AIUnitModel executor, BoardState state, out ActionArgs args) => m_actionResolver.TryResolve(this, executor, state, out args);
    }
}
