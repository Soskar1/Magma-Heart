using System;

namespace MagmaHeart.AI.Actions
{
    public class ActionDefinition
    {
        public Type ActionType { get; init; }
        public ActionData Data { get; init; }

        public ActionDefinition(Type actionType, ActionData data)
        {
            ActionType = actionType;
            Data = data;
        }
    }
}
