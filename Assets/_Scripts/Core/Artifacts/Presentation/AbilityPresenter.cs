using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AbilityPresenter : MonoBehaviour
    {
        private Image m_image;
        private Button m_button;
        private AbilityDefinition m_ability;
        private PlayerTurnController m_turnController;
        private IGameWorld m_gameWorld;
        private EntityModel m_executor;
        
        private ResourceCost m_minimalResourceCost;

        private void Awake()
        {
            m_image = GetComponent<Image>();
            m_button = GetComponent<Button>();
        }

        public void Initialize(Artifact artifact, PlayerTurnController turnController, EntityModel executor, IGameWorld gameWorld)
        {
            m_ability = artifact.Data.AbilityDefinition;
            m_turnController = turnController;
            m_image.sprite = artifact.Data.Icon;
            m_gameWorld = gameWorld;
            m_executor = executor;

            m_minimalResourceCost = m_ability.ComputeCost(gameWorld, executor.Id, AbilityTarget.None);

            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executor.Id, resource.Id).OnParameterValueChanged += TriggerValidation;

            m_turnController.OnCanExecuteActionsChanged += HandleOnCanExecuteActionsChanged;
            m_turnController.OnTurnStarted += TriggerValidation;

            Validate();
        }

        private void OnDisable()
        {
            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executor.Id, resource.Id).OnParameterValueChanged -= TriggerValidation;

            m_turnController.OnCanExecuteActionsChanged -= HandleOnCanExecuteActionsChanged;
            m_turnController.OnTurnStarted -= TriggerValidation;
        }

        private void TriggerValidation(object _, EventArgs __) => Validate();

        private void HandleOnCanExecuteActionsChanged(object _, OnCanExecuteActionsChangedEventArgs args)
        {
            if (!args.CanExecute)
                m_button.interactable = false;
        }

        private void Validate()
        {
            if (m_executor.GetCooldown(m_ability.Id) > 0)
            {
                m_button.interactable = false;
                return;
            }

            foreach (var resource in m_minimalResourceCost.GetAllCosts())
            {
                IParameter parameter = m_gameWorld.GetParameter(m_executor.Id, resource.Id);
                
                if (parameter.CurrentValue < resource.Amount)
                {
                    m_button.interactable = false;
                    return;
                }
            }

            m_button.interactable = true;
        }

        public void OnAbilityButtonPressed()
        {
            m_turnController.ArmAbility(m_ability);
        }
    }
}