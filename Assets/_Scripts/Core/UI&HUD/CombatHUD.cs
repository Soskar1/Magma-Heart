using System.Collections.Generic;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class CombatHUD : MonoBehaviour, IDisplayable
    {
        [SerializeField] private GameObject m_energyHUD;
        [SerializeField] private GameObject m_energyCrystalPrefab;
        [SerializeField] private Sprite m_emptyEnergyCrystalGFX;
        [SerializeField] private Sprite m_activeEnergyCrystalGFX;

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

        public void Show()
        {
            for (int i = 0; i < m_playerEnergy.CurrentEnergy; ++i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;

            m_energyHUD.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_energyHUD.gameObject.SetActive(false);
        }
    }
}