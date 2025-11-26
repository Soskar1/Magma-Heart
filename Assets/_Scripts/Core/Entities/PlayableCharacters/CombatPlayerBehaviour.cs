using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Input;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour
    {
        private readonly CombatUserInput m_userInput;
        
        private readonly PlayerTurnContext m_combatController;
        
        private readonly EntityAnimation m_animation;

        public CombatPlayerBehaviour(Player player, CombatUserInput userInput)
        {
            m_combatController = (PlayerTurnContext)player.TurnContext;
            m_userInput = userInput;

            m_animation = player.Animation;
        }

        public void Enable()
        {
            m_animation.PlayIdleAnimation();
        }

        public void Disable()
        {

        }

        public void Update()
        {
            if (!m_combatController.CanExecuteActions)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }
    }
}