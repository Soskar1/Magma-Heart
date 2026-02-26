using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;

namespace MagmaHeart.Core.Entities
{
    public class StartOfTurnCommandFactory : IStartOfTurnCommandFactory
    {
        public IEnumerable<IBoardCommand> BuildStartOfTurnCommands(Board board, AIUnitModel unit)
        {
            EntityModel entityModel = unit as EntityModel;
            int newEnergyValue = entityModel.Energy.CurrentEnergy + entityModel.Energy.EnergyRegenerationPerTurn;
            throw new System.Exception("FIX THIS: Energy regeneration is not implemented yet");
            return new List<IBoardCommand>()
            {
                // new UpdateEnergyCommand(entityModel.Id, newEnergyValue)
            };
        }
    }
}