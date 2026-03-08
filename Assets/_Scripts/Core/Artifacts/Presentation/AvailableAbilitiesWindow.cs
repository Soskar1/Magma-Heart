using MagmaHeart.Abilities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AvailableAbilitiesWindow : MonoBehaviour
    {
        [SerializeField] private AbilityPresenter m_abilityPresenterPrefab;
        [SerializeField] private Transform m_abilityContainer;
        private Inventory m_inventory;
        private PlayerTurnController m_turnController;
        private int m_executorId;
        private IGameWorld m_gameWorld;

        public void Initialize(Inventory inventory, PlayerTurnController playerTurnController, int executorId, IGameWorld gameWorld)
        {
            m_inventory = inventory;
            m_turnController = playerTurnController;
            m_executorId = executorId;
            m_gameWorld = gameWorld;

            inventory.OnArtifactPicked += HandleOnArtifactPicked;
        }

        private void OnDisable()
        {
            m_inventory.OnArtifactPicked -= HandleOnArtifactPicked;
        }

        private void HandleOnArtifactPicked(object _, Artifact artifact)
        {
            if (artifact.Data.AbilityDefinition != null)
                Add(artifact);
        }

        private void Add(Artifact artifact)
        {
            AbilityPresenter instance = Instantiate(m_abilityPresenterPrefab, m_abilityContainer);
            
            instance.Initialize(artifact, m_turnController, m_executorId, m_gameWorld);
        }
    }
}
