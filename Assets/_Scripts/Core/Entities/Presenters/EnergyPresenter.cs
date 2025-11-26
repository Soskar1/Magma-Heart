using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.UI;
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

        private List<Image> m_crystalVisuals;
        private EnergyModel m_playerEnergy;

        public void Initialize(Player player)
        {
            m_crystalVisuals = new List<Image>();
            m_playerEnergy = player.Energy;

            for (int i = 0; i < m_playerEnergy.MaxEnergy; ++i)
            {
                GameObject energyCrystalInstance = Instantiate(m_energyCrystalPrefab);
                energyCrystalInstance.transform.SetParent(m_energyHUD.transform, false);
                m_crystalVisuals.Add(energyCrystalInstance.GetComponent<Image>());
            }

            m_playerEnergy.OnEnergyChanged += DisplayEnergy;
            m_playerEnergy.OnPreviewEnergyChanged += DisplayEnergyPrice;
        }

        public void OnDisable()
        {
            m_playerEnergy.OnEnergyChanged -= DisplayEnergy;
            m_playerEnergy.OnPreviewEnergyChanged -= DisplayEnergyPrice;
        }

        private void DisplayEnergy(object obj, OnEnergyChangedEventArgs args)
        {
            for (int i = 0; i < args.CurrentEnergy; ++i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;

            for (int i = args.CurrentEnergy; i < m_crystalVisuals.Count; ++i)
                m_crystalVisuals[i].sprite = m_emptyEnergyCrystalGFX;
        }

        private void DisplayEnergyPrice(object obj, OnPreviewEnergyChangedEventArgs args)
        {
            for (int i = args.CurrentEnergy; i > args.CurrentEnergy - args.PreviewCost; --i)
                m_crystalVisuals[i - 1].sprite = m_priceEnergyCrystalGFX;

            for (int i = args.CurrentEnergy - args.PreviewCost - 1; i >= 0; --i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;
        }

        public void Show() => m_energyHUD.gameObject.SetActive(true);
        public void Hide() => m_energyHUD.gameObject.SetActive(false);
    }
}