using MagmaHeart.AI.States;
using System;

namespace MagmaHeart.AI.Actions
{
    public class ActionDefinition
    {
        public Type ActionType { get; init; }
        public ActionData Data { get; init; }
        public IActionArgumentCreator ArgumentCreator { get; init; }

        public ActionDefinition(Type actionType, ActionData data, IActionArgumentCreator argumentCreator)
        {
            ActionType = actionType;
            Data = data;
            ArgumentCreator = argumentCreator;
        }

        public ActionArgs CreateArguments(AIUnitModel executor, AIUnitModel target, BoardState state) => ArgumentCreator.CreateArguments(Data, executor, target, state);
    }
}
