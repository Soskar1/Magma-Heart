using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_abilityName;
        [SerializeField] private TextMeshProUGUI m_abilityDescription;
        [SerializeField] private List<GameObject> m_energyCrystalIcons;
        [SerializeField] private GameObject m_magmaHeartIcon;

        [SerializeField] private List<GameObject> m_cooldownSectors;
        [SerializeField] private TextMeshProUGUI m_cooldownText;

        [SerializeField] private ParameterId m_energy;
        [SerializeField] private ParameterId m_magmaHeart;

        public void Present(AbilityDefinition abilityDefinition, ResourceCost minimalResourceCost)
        {
            m_abilityName.text = abilityDefinition.Id;
            m_abilityDescription.text = abilityDefinition.Description;

            foreach (var cost in minimalResourceCost.GetAllCosts())
            {
                if (cost.Id == m_energy)
                {
                    for (int i = 0; i < cost.Amount; ++i)
                        m_energyCrystalIcons[i].gameObject.SetActive(true);

                    for (int i = cost.Amount; i < m_energyCrystalIcons.Count; ++i)
                        m_energyCrystalIcons[i].gameObject.SetActive(false);
                }
                
                if (cost.Id == m_magmaHeart)
                    m_magmaHeartIcon.SetActive(true);
            }

            m_cooldownText.text = abilityDefinition.CooldownTurns.ToString();

            for (int i = 0; i < abilityDefinition.CooldownTurns; ++i)
                m_cooldownSectors[i].gameObject.SetActive(true);

            for (int i = abilityDefinition.CooldownTurns; i < m_cooldownSectors.Count; ++i)
                m_cooldownSectors[i].gameObject.SetActive(false);

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_magmaHeartIcon.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}