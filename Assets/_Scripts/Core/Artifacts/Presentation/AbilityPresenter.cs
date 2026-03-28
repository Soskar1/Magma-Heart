using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_cooldownText;
        [SerializeField] private GameObject m_vfxSpawnpoint;
        [SerializeField] private Image m_selectedEffect;

        [SerializeField] private float m_audioFadeDuration = 1f;
        [SerializeField] private float m_audioMaxVolume = 0.5f;

        private Image m_image;
        private Button m_button;
        private AbilityDefinition m_ability;
        private PlayerTurnController m_turnController;
        private IGameWorld m_gameWorld;
        private EntityModel m_executor;
        private ParticleSystem m_vfx;

        private AudioSource m_audio;
        
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
            m_image.sprite = artifact.Data.AbilityIcon;
            m_gameWorld = gameWorld;
            m_executor = executor;

            m_audio = GetComponent<AudioSource>();

            if (artifact.Data.ChargingSfx != null)
                m_audio.clip = artifact.Data.ChargingSfx;

            if (artifact.Data.AbilityWindowVfx != null)
            {
                m_vfx = Instantiate(artifact.Data.AbilityWindowVfx, transform);

                m_vfx.transform.position = m_vfxSpawnpoint.transform.position;
                m_vfx.transform.localScale = Vector3.one;
            }

            m_minimalResourceCost = m_ability.ComputeCost(gameWorld, executor.Id, AbilityTarget.None);

            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executor.Id, resource.Id).OnParameterValueChanged += HandleOnParameterValueChanged;

            m_turnController.OnCanExecuteActionsChanged += HandleOnCanExecuteActionsChanged;
            m_turnController.OnAbilityDisarmed += HandleOnAbilityDisarm;
            m_executor.OnCooldownChanged += HandleOnCooldownChanged;

            Validate();
        }

        private void OnDisable()
        {
            foreach (var resource in m_minimalResourceCost.GetAllCosts())
                m_gameWorld.GetParameter(m_executor.Id, resource.Id).OnParameterValueChanged -= HandleOnParameterValueChanged;

            m_turnController.OnCanExecuteActionsChanged -= HandleOnCanExecuteActionsChanged;
            m_turnController.OnAbilityDisarmed -= HandleOnAbilityDisarm;
            m_executor.OnCooldownChanged -= HandleOnCooldownChanged;
        }

        private void HandleOnCooldownChanged(object _, OnCooldownChangedEventArgs args)
        {
            if (args.AbilityId != m_ability.Id)
                return;

            m_cooldownText.text = args.CurrentCooldown > 0 ? args.CurrentCooldown.ToString() : string.Empty;
            Validate();
        }

        private void HandleOnCanExecuteActionsChanged(object _, OnCanExecuteActionsChangedEventArgs args)
        {
            if (!args.CanExecute)
                Deactivate();
        }
        
        private void HandleOnParameterValueChanged(object _, EventArgs __) => Validate();

        private void Validate()
        {
            if (m_executor.GetCooldown(m_ability.Id) > 0)
            {
                Deactivate();
                return;
            }

            foreach (var resource in m_minimalResourceCost.GetAllCosts())
            {
                IParameter parameter = m_gameWorld.GetParameter(m_executor.Id, resource.Id);
                
                if (parameter.CurrentValue < resource.Amount)
                {
                    Deactivate();
                    return;
                }
            }

            m_button.interactable = true;

            if (m_vfx != null)
                m_vfx.Play();
        }

        public void OnAbilityButtonPressed()
        {
            m_turnController.ArmAbility(m_ability);
            m_selectedEffect.enabled = true;
            m_audio.Play();
            StartCoroutine(FadeAudio(m_audioMaxVolume));
        }

        private void Deactivate()
        {
            m_button.interactable = false;
            m_selectedEffect.enabled = false;

            if (m_vfx != null)
                m_vfx.Stop();

            StartCoroutine(FadeAudio(0));
        }

        private void HandleOnAbilityDisarm(object _, EventArgs __)
        {
            m_selectedEffect.enabled = false;
            StartCoroutine(FadeAudio(0));
        }

        private IEnumerator FadeAudio(float targetVolume)
        {
            float startVolume = m_audio.volume;
            float time = 0f;

            while (time < m_audioFadeDuration)
            {
                time += Time.deltaTime;
                m_audio.volume = Mathf.Lerp(startVolume, targetVolume, time / m_audioFadeDuration);
                yield return null;
            }

            m_audio.volume = targetVolume;

            if (targetVolume == 0f)
                m_audio.Stop();
        }
    }
}