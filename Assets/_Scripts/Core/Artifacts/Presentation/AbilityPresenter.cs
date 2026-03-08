using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.Core.Entities.PlayableCharacters;
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
        private int m_executorId;
        
        private ResourceCost m_minimalResourceCost;

        private void Awake()
        {
            m_image = GetComponent<Image>();
            m_button = GetComponent<Button>();
        }

        public void Initialize(Artifact artifact, PlayerTurnController turnController, int executorId, IGameWorld gameWorld)
        {
            m_ability = artifact.Data.AbilityDefinition;
            m_turnController = turnController;
            m_image.sprite = artifact.Data.Icon;
            m_gameWorld = gameWorld;
            m_executorId = executorId;

            m_minimalResourceCost = m_ability.ComputeCost(gameWorld, executorId, AbilityTarget.None);

            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executorId, resource.Id).OnParameterValueChanged += HandleOnParameterValueChanged;

            m_turnController.OnCanExecuteActionsChanged += HandleOnCanExecuteActionsChanged;

            Validate();
        }

        private void OnDisable()
        {
            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executorId, resource.Id).OnParameterValueChanged -= HandleOnParameterValueChanged;

            m_turnController.OnCanExecuteActionsChanged -= HandleOnCanExecuteActionsChanged;
        }

        private void HandleOnParameterValueChanged(object _, OnParameterValueChangedEventArgs __) => Validate();

        private void HandleOnCanExecuteActionsChanged(object _, OnCanExecuteActionsChangedEventArgs args)
        {
            if (!args.CanExecute)
                m_button.interactable = false;
        }

        private void Validate()
        {
            foreach (var resource in m_minimalResourceCost.GetAllCosts())
            {
                IParameter parameter = m_gameWorld.GetParameter(m_executorId, resource.Id);
                
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