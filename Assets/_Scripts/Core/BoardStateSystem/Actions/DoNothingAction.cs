using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    internal class DoNothingAction : CombatAction<ActionArgs>
    {
        public DoNothingAction(EntityModel actionPossessor) : base(actionPossessor) { }

        public override IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnit unit) => new List<ActionArgs>() { ActionArgs.Empty };

        public override int GetEnergyCost(ActionArgs args, BoardState boardState) => 0;
    }
}
