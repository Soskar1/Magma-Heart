using MagmaHeart.AI.States;
using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    public class ActionDefinition
    {
        public Type ActionType { get; init; }
        public ActionData Data { get; init; }
        public IActionArgumentCreator ArgumentCreator { get; init; }
        public IActionTargetSelector TargetSelector { get; init; }

        public ActionDefinition(Type actionType, ActionData data, IActionArgumentCreator argumentCreator, IActionTargetSelector targetSelector)
        {
            ActionType = actionType;
            Data = data;
            TargetSelector = targetSelector;
            ArgumentCreator = argumentCreator;
        }

        public IEnumerable<ActionArgs> CreateArguments(AIUnitModel executor, AIUnitModel target, BoardState state) => ArgumentCreator.CreateArguments(Data, executor, target, state);
    }
}
