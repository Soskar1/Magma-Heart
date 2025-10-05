using MagmaHeart.Core.Input;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class RewardPlayerBehaviour : IPlayerBehaviour
    {
        private readonly UserInput m_userInput;
        private readonly PlayerAnimation m_animation;

        public RewardPlayerBehaviour(UserInput userInput, PlayerAnimation animation)
        {
            m_userInput = userInput;
            m_animation = animation;
        }

        public void Enable()
        {
            m_userInput.Disable();
            m_animation.PlayIdleAnimation();
        }

        public void Disable() => m_userInput.Enable();

        public void Update() { }
    }
}