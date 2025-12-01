namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour
    {
        private readonly EntityAnimation m_animation;

        public CombatPlayerBehaviour(Player player)
        {
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
            
        }
    }
}