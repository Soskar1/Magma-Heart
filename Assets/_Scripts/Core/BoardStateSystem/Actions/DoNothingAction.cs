using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class DoNothingAction : CombatAction<ActionArgs<EntityModel>>
    {
        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, IEnumerable<AIUnitModel> targets) => new List<ActionArgs>() { new ActionArgs(executor) };

        public override int GetEnergyCost(ActionArgs<EntityModel> args, BoardState boardState) => 0;
    }
}
