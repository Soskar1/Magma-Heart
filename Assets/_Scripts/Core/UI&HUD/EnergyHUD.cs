using System.Collections.Generic;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class EnergyHUD : MonoBehaviour, IDisplayable
    {
        [SerializeField] private GameObject m_energyHUD;
        [SerializeField] private GameObject m_energyCrystalPrefab;
        [SerializeField] private Sprite m_emptyEnergyCrystalGFX;
        [SerializeField] private Sprite m_activeEnergyCrystalGFX;
        [SerializeField] private Sprite m_priceEnergyCrystalGFX;

        private List<Image> m_crystalVisuals;
        private Energy m_playerEnergy;

        public void Initialize(Player player)
        {
            m_crystalVisuals = new List<Image>();
            m_playerEnergy = player.Energy;

            for (int i = 0; i < player.Stats.MaxEnergy; ++i)
            {
                GameObject energyCrystalInstance = Instantiate(m_energyCrystalPrefab);
                energyCrystalInstance.transform.SetParent(m_energyHUD.transform, false);
                m_crystalVisuals.Add(energyCrystalInstance.GetComponent<Image>());
            }
        }

        public void DisplayEnergy()
        {
            for (int i = 0; i < m_playerEnergy.CurrentEnergy; ++i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;

            for (int i = m_playerEnergy.CurrentEnergy; i < m_crystalVisuals.Count; ++i)
                m_crystalVisuals[i].sprite = m_emptyEnergyCrystalGFX;
        }

        public void DisplayEnergyPrice(int energyAmount)
        {
            for (int i = m_playerEnergy.CurrentEnergy; i > m_playerEnergy.CurrentEnergy - energyAmount; --i)
                m_crystalVisuals[i - 1].sprite = m_priceEnergyCrystalGFX;

            for (int i = m_playerEnergy.CurrentEnergy - energyAmount - 1; i >= 0; --i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;
        }

        public void Show() => m_energyHUD.gameObject.SetActive(true);

        public void Hide() => m_energyHUD.gameObject.SetActive(false);
    }
}