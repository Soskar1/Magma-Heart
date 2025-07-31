using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour
    {
        private UserInput m_userInput;

        public TurnBasedPlayerBehaviour(UserInput userInput)
        {
            m_userInput = userInput;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.Enable();
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.Disable();
        }

        public void Update()
        {
        }
    }
}