using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.PlayableCharacters;
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

        private List<Image> m_crystalVisuals;
        private Player m_player;
        private EnergyModel m_playerEnergy;

        public void Initialize(Player player)
        { 
            m_player = player;
            m_crystalVisuals = new List<Image>();
            m_playerEnergy = player.Energy;

            for (int i = 0; i < m_playerEnergy.MaxEnergy; ++i)
            {
                GameObject energyCrystalInstance = Instantiate(m_energyCrystalPrefab);
                energyCrystalInstance.transform.SetParent(m_energyHUD.transform, false);
                m_crystalVisuals.Add(energyCrystalInstance.GetComponent<Image>());
            }

            m_playerEnergy.OnEnergyChanged += HandleOnEnergyChanged;
            m_player.CombatController.OnPreviewChanged += HandleOnActionPreviewChanged;
        }

        public void OnDisable()
        {
            m_playerEnergy.OnEnergyChanged -= HandleOnEnergyChanged;
            m_player.CombatController.OnPreviewChanged -= HandleOnActionPreviewChanged;
        }

        private void HandleOnEnergyChanged(object obj, OnEnergyChangedEventArgs args) => DisplayEnergy(args.CurrentEnergy);

        private void DisplayEnergy(int currentEnergy)
        {
            for (int i = 0; i < currentEnergy; ++i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;

            for (int i = currentEnergy; i < m_crystalVisuals.Count; ++i)
                m_crystalVisuals[i].sprite = m_emptyEnergyCrystalGFX;
        }

        private void HandleOnActionPreviewChanged(object obj, OnActionPreviewChangedEventArgs args)
        {
            int currentEnergy = m_playerEnergy.CurrentEnergy;

            if (args.ActionPreview == null)
            {
                DisplayEnergy(currentEnergy);
                return;
            }

            int cost = args.ActionPreview.EnergyCost;

            for (int i = currentEnergy; i > currentEnergy - cost; --i)
                m_crystalVisuals[i - 1].sprite = m_priceEnergyCrystalGFX;

            for (int i = currentEnergy - cost - 1; i >= 0; --i)
                m_crystalVisuals[i].sprite = m_activeEnergyCrystalGFX;
        }

        public void Show() => m_energyHUD.gameObject.SetActive(true);
        public void Hide() => m_energyHUD.gameObject.SetActive(false);
    }
}