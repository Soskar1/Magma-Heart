namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedUserInput
    {
        private UserInput m_userInput;

        public MouseControl MouseControl { get; private set; }

        public TurnBasedUserInput(UserInput userInput, MouseControl mouseControl)
        {
            m_userInput = userInput;
            MouseControl = mouseControl;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.Enable();
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.Disable();
        }
    }
}