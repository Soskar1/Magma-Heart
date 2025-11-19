using MagmaHeart.AI.States;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record TriggerAttackEventStateChange : MagmaHeartStateChange
    {
        public override void ApplyChangeToActualState(CombatBoardState actualBoard)
        {
            // Trigger on attack event

            //OnAttackEventArgs attackArgs = new OnAttackEventArgs(model);
            //OnAttackTriggered?.Invoke(this, attackArgs);
            //if (OnAttackTriggered == null)
            //{
            //    Debug.LogWarning("No one is subscribed to the OnAttackTriggered event. Executing Hit()");
            //    Hit(model);
            //}

            throw new System.NotImplementedException();
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation) { }
    }
}
