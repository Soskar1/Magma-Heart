using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using System.Collections.Generic;

namespace MagmaHeart.Core.Entities
{
    public class StartOfTurnCommandFactory : IStartOfTurnCommandFactory
    {
        public IEnumerable<IBoardCommand> BuildStartOfTurnCommands(Board board, AIUnitModel unit)
        {
            EntityModel entityModel = unit as EntityModel;
            int newEnergyValue = entityModel.Energy.CurrentEnergy + entityModel.Energy.EnergyRegenerationPerTurn;
            return new List<IBoardCommand>()
            {
                new UpdateEnergyCommand(entityModel.Id, newEnergyValue)
            };
        }
    }
}