using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities
{
    public abstract class EntityTurnContext : TurnContext<EntityModel>
    {
        public Room CurrentRoom => CurrentCombatBoardState.Room;
        public CombatBoardState CurrentCombatBoardState { get; private set; }

        public EntityTurnContext(EntityModel model) : base(model) { }

        public virtual void StartBattle(CombatBoardState combatBoardState)
        {
            CurrentCombatBoardState = combatBoardState;
        }

        public virtual void EndBattle()
        {
            TypedModel.Energy.Reset();
            CurrentCombatBoardState = null;
        }

        public abstract Task StartTurnTask();
        public abstract void EndTurn();

        public override IEnumerable<StateChange> ProduceStartTurnChanges()
        {
            int newEnergyValue = TypedModel.Energy.CurrentEnergy + TypedModel.Stats.EnergyRegenerationPerTurn;
            return new List<StateChange>()
            {
                new UpdateEnergyStateChange(TypedModel, newEnergyValue)
            };
        }
    }
}