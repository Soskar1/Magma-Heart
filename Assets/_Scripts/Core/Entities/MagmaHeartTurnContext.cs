using MagmaHeart.AI;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using System.Collections.Generic;

namespace MagmaHeart.Core.Entities
{
    public class MagmaHeartTurnContext : TurnContext
    {
        public override IEnumerable<StateChange> ProduceStartTurnChanges(AIUnitModel model)
        {
            EntityModel entityModel = model as EntityModel;
            int newEnergyValue = entityModel.Energy.CurrentEnergy + entityModel.Energy.EnergyRegenerationPerTurn;
            return new List<StateChange>()
            {
                new UpdateEnergyStateChange(entityModel, newEnergyValue)
            };
        }
    }
}