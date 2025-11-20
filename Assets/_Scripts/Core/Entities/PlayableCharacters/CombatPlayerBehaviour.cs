using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Input;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour
    {
        private readonly CombatUserInput m_userInput;
        
        private readonly PlayerCombatController m_combatController;
        
        private readonly Facing m_facing;
        private readonly EntityAnimation m_animation;

        private readonly TurnBasedMovement m_movement;

        public CombatPlayerBehaviour(Player player, CombatUserInput userInput)
        {
            m_combatController = (PlayerCombatController)player.CombatController;
            m_userInput = userInput;

            m_facing = player.Facing;
            m_animation = player.Animation;
            m_movement = player.TurnBasedMovement;
        }

        public void Enable()
        {
            m_movement.OnMovementStarted += HandleOnMovementStarted;
            m_movement.OnMovementEnded += HandleOnMovementEnded;

            m_animation.PlayIdleAnimation();
        }

        public void Disable()
        {
            m_movement.OnMovementStarted -= HandleOnMovementStarted;
            m_movement.OnMovementEnded -= HandleOnMovementEnded;
        }

        public void Update()
        {
            if (!m_combatController.CanExecuteActions)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }
        
        private void HandleOnMovementStarted(object obj, OnMovementEventArgs e)
        {
            m_combatController.CanExecuteActions = false;
            m_animation.PlayRunAnimation();

            m_facing.TryUpdateFacing((e.To - e.From).x);
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            m_combatController.CanExecuteActions = true;
            m_animation.PlayIdleAnimation();
        }
    }
}