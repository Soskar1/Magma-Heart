using MagmaHeart.Abilities.Resources;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Presentation.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EnergyPresenter : MonoBehaviour, IDisplayable
    {
        [SerializeField] private GameObject m_energyHUD;
        [SerializeField] private GameObject m_energyCrystalPrefab;
        [SerializeField] private Sprite m_emptyEnergyCrystalGFX;
        [SerializeField] private Sprite m_activeEnergyCrystalGFX;
        [SerializeField] private Sprite m_priceEnergyCrystalGFX;

        // TODO: I do not like this shit. We need to make it more cleaner. Good for prototype though
        [SerializeField] private ResourceId m_resourceId;
        public ResourceId Resource => m_resourceId;

        private List<Image> m_crystalVisuals;
        private EnergyModel m_playerEnergy;

        public void Initialize(Entity player)
        { 
            m_crystalVisuals = new List<Image>();
            m_playerEnergy = player.Energy;

            for (int i = 0; i < m_playerEnergy.MaxEnergy; ++i)
            {
                GameObject energyCrystalInstance = Instantiate(m_energyCrystalPrefab);
                energyCrystalInstance.transform.SetParent(m_energyHUD.transform, false);
                m_crystalVisuals.Add(energyCrystalInstance.GetComponent<Image>());
            }

            m_playerEnergy.OnEnergyChanged += HandleOnEnergyChanged;
        }

        public void OnDisable()
        {
            m_playerEnergy.OnEnergyChanged -= HandleOnEnergyChanged;
        }

        private void HandleOnEnergyChanged(object obj, OnEnergyChangedEventArgs args) => DisplayCurrentEnergy();

        public void DisplayCurrentEnergy()
        {
            int currentEnergy = m_playerEnergy.CurrentEnergy;

            for (int i = 0; i < currentEnergy; ++i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;

            for (int i = currentEnergy; i < m_crystalVisuals.Count; ++i)
                m_crystalVisuals[i].sprite = m_emptyEnergyCrystalGFX;
        }

        public void DisplayCost(int cost)
        {
            int currentEnergy = m_playerEnergy.CurrentEnergy;

            for (int i = currentEnergy; i > currentEnergy - cost; --i)
                m_crystalVisuals[i - 1].sprite = m_priceEnergyCrystalGFX;

            for (int i = currentEnergy - cost - 1; i >= 0; --i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;
        }

        public void Show() => m_energyHUD.gameObject.SetActive(true);
        public void Hide() => m_energyHUD.gameObject.SetActive(false);
    }
}