using UnityEngine;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AvailableAbilitiesWindow : MonoBehaviour
    {
        [SerializeField] private AbilityPresenter m_abilityPresenterPrefab;
        [SerializeField] private Transform m_abilityContainer;
        private Inventory m_inventory;

        public void Initialize(Inventory inventory)
        {
            m_inventory = inventory;
            
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
            
            instance.Initialize(artifact);
        }
    }
}
