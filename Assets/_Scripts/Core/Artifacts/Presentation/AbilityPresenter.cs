using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        private Artifact m_artifact;
        private PlayerTurnController m_turnController;

        public void Initialize(Artifact artifact, PlayerTurnController turnController)
        {
            m_artifact = artifact;
            m_turnController = turnController;
            m_image.sprite = m_artifact.Data.Icon;
        }

        public void OnAbilityButtonPressed()
        {
            m_turnController.ArmAbility(m_artifact.Data.AbilityDefinition);
        }
    }
}